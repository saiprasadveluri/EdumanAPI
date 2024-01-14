using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class IndividualProgressReportController : ControllerBase
    {
        EduManDBContext context = null;
        public IndividualProgressReportController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetReport(Guid ExamId, Guid StuMapId)
        {
            bool Success = false;
            IndividualProgressReportDTO dto = new IndividualProgressReportDTO();
            try
            {
                var StuInfo = (from obj in context.StuStdAcdYearMaps
                               join Stuobj in context.StudentInfos on obj.StuId equals Stuobj.StuId
                               join StnObj in context.Standards on obj.StnId equals StnObj.StdId
                               where obj.MapId == StuMapId
                               select new { Stuobj.RegdNo, Stuobj.FName, StnObj.StdName }).FirstOrDefault();
                
                dto.RegdNo = StuInfo.RegdNo;
                dto.StuName = StuInfo.FName;
                dto.StnName = StuInfo.StdName;

                var ExamInfo = (from obj in context.ExamSchedules
                                join eobj in context.Exams on obj.ExamId equals eobj.ExamId
                                join SSmapobj in context.StdSubMaps on obj.MapId equals SSmapobj.MapId
                                join SubObj in context.Subjects on SSmapobj.SubId equals SubObj.SubId
                                where eobj.ExamId == ExamId
                                select new { obj.ExamSchId, SubObj.SubjectName }).ToList();
                
                foreach (var sch in ExamInfo)
                {
                    IndividualProgressReportSubject sub = new IndividualProgressReportSubject();
                    sub.SubjectName = sch.SubjectName;
                    dto.Subjects.Add(sub);

                    var repinfo = (from obj in context.ExamProgressReports
                                   join hobj in context.ExamProgressReportHeads on obj.PRHeadId equals hobj.PRHId
                                   where obj.SchId == sch.ExamSchId && obj.StuMapId == StuMapId
                                   select new { HeadName = hobj.HeadName, obj.Marks,hobj.MaxMarks,hobj.MinMarks }).ToList();
                    foreach (var rep in repinfo)
                    {
                        IndividualProgressReportSubjectMark mark = new IndividualProgressReportSubjectMark();
                        mark.HeadName = rep.HeadName;
                        mark.Marks = rep.Marks;
                        mark.Maxmarks = rep.MaxMarks;
                        mark.Minmarks = rep.MinMarks;

                        sub.SubMarks.Add(mark);
                    }
                }
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
                return BadRequest("Service error");
            }
        }
    }
}
