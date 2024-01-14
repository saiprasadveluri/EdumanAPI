using EduManAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class TimetableForMobileAppController : ControllerBase
    {
        EduManDBContext context = null;
        IGeneratePdf generatePdf = null;

        public TimetableForMobileAppController(EduManDBContext ctx, IGeneratePdf genPdf)
        {
            generatePdf = genPdf;
            context = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> GetTitmetable(Guid StnId)
        {
            bool Success = false;
            List<ViewTimeTableEntryDTO> dto = new List<ViewTimeTableEntryDTO>();
            try
            {
                dto = (from obj in context.TimeTables
                       join mapobj in context.StdSubMaps on obj.StnSubMapId equals mapobj.MapId
                       join sobj in context.Subjects on mapobj.SubId equals sobj.SubId
                       join tobj in context.Teachers on obj.TeacherId equals tobj.TeacherId
                       where mapobj.StdId == StnId
                       select new ViewTimeTableEntryDTO
                       {
                           HourNo = obj.HourNo,
                           WeekNo = obj.WeekNo,
                           SubName = sobj.SubjectName,
                           TeacherName = tobj.FName
                       }
                            ).ToList().OrderBy(s => s.WeekNo).ThenBy(s => s.HourNo).ToList(); ;
                Success = true;
            }
            catch (Exception e)
            {

            }
            var model = PopulateTimetableData(dto);
            return await generatePdf.GetPdf("Timetable", model);
        }

        private TimetableViewVM PopulateTimetableData(List<ViewTimeTableEntryDTO> dto)
        {
            //List<ViewTimeTableEntryVM> model = new List<ViewTimeTableEntryVM>();
            TimetableViewVM vm = new TimetableViewVM();
            try
            {
                   if (dto.Count>0)
                    {
                        var WDayLst = dto.Select(s => s.WeekNo).Distinct().OrderBy(s => s).ToList();
                        var WHourLst = dto.Select(s => s.HourNo).Distinct().OrderBy(s => s).ToList();

                        vm.WDaysList = WDayLst;
                        vm.WHourList = WHourLst;

                        foreach (var wd in WDayLst)
                        {
                            TimetableViewHourData data1 = new TimetableViewHourData();
                            data1.WNo = wd;
                            vm.DayDataLst.Add(data1);
                            var recTemp1 = dto.Where(s => (s.WeekNo == wd)).ToList();
                            foreach (var wh in WHourLst)
                            {
                                var rec = recTemp1.Where(s => s.HourNo == wh).FirstOrDefault();

                                if (rec != null)
                                {
                                    ViewTimeTableEntryVM entry = new ViewTimeTableEntryVM();
                                    entry.SubName = rec.SubName;
                                    entry.TeacherName = rec.TeacherName;
                                    entry.WeekNo = rec.WeekNo;
                                    entry.HourNo = rec.HourNo;
                                    data1.HourDataLst.Add(entry);
                                }
                            }
                        }
                    }
                
            }
            catch (Exception e)
            {

            }
            return vm;
        }
    }
}
