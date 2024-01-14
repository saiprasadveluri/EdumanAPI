using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class TimeTableController : ControllerBase
    {
        EduManDBContext context = null;
        public TimeTableController(EduManDBContext ctx)
        {
            context = ctx;
        }


        [HttpGet]
        public ActionResult GetTitmetable(Guid StnId)
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
            if (Success)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
             }
        }

        [HttpPost]
        public ActionResult UploadTimeTable(AddTimeTableDTO dto)
        {
            bool Success = false;
            string ErrMsg = "";
            try
            {
                Guid StdId = dto.StnId;
                Guid OrgId = dto.OrgId;

                //Delete Exiting TitmeTable before creating a new
                var recs = (from obj in context.TimeTables
                            join mapobj in context.StdSubMaps on obj.StnSubMapId equals mapobj.MapId
                            where mapobj.StdId == StdId
                            select obj).ToList();
                context.TimeTables.RemoveRange(recs);
                context.SaveChanges();
                //End of Delete Exiting TitmeTable.
                Log.Information("Old Timetable Deleted");
            //Fetch Lsit of mapped subjects for this standard
            var MapLst = (from obj in context.StdSubMaps
                             join subObj in context.Subjects on obj.SubId equals subObj.SubId
                             where obj.StdId == StdId
                             select new {obj.MapId, subObj.SubCode}).ToList();

                var TeachLst = (from obj in context.Teachers
                                where obj.OrgId == OrgId
                                select obj).ToList();
                Log.Information("maplist len: " + MapLst.Count);
                Log.Information("TeachLst len: " + TeachLst.Count);
                foreach (var rec in dto.entries)
                {
                    TimeTable tt = new TimeTable();
                    tt.TTID = Guid.NewGuid();
                    tt.WeekNo = rec.WeekNo;
                    tt.HourNo = rec.HourNo;

                    tt.StnSubMapId = MapLst.Where(m => m.SubCode == rec.SubCode).Select(m=>m.MapId).FirstOrDefault();
                    tt.TeacherId = TeachLst.Where(t => t.EmpId == rec.TeacherEmpId).Select(t => t.TeacherId).FirstOrDefault();
                    Log.Information("Subcode-:" + rec.SubCode + " - TCode " + rec.TeacherEmpId);
                        Log.Information("Subcode-mapid:" + tt.StnSubMapId + " - " + rec.SubCode);
                        //Log.Fatal("TeacherId not found for Sub Code:" + rec.TeacherEmpId);
                    context.TimeTables.Add(tt);
                   
            }
                context.SaveChanges();
                Log.Information("Table Saved");
                Success = true;
           }
            catch (Exception e)
            {
                ErrMsg+= e.Message;
            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest(ErrMsg);
            }
        }
    }
}
