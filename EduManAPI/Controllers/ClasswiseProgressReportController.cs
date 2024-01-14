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
    public class ClasswiseProgressReportController : ControllerBase
    {
        EduManDBContext context = null;
        public ClasswiseProgressReportController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetReport(Guid ExamId)
        {
            bool Success = false;
            ClasswiseProgressReportDTO CWdto = new ClasswiseProgressReportDTO();
            
            try
            {
                var ExamInfo = (from obj in context.ExamSchedules
                                join eobj in context.Exams on obj.ExamId equals eobj.ExamId
                                join SSmapobj in context.StdSubMaps on obj.MapId equals SSmapobj.MapId
                                join SubObj in context.Subjects on SSmapobj.SubId equals SubObj.SubId
                                join StnObj in context.Standards on SSmapobj.StdId equals StnObj.StdId
                                where eobj.ExamId == ExamId
                                select new { StnObj.StdId, obj.ExamSchId, SubObj.SubjectName }).ToList();

                var StuMapIds = (from obj in context.Exams
                                 join schobj in context.ExamSchedules on obj.ExamId equals schobj.ExamId
                                 join repobj in context.ExamProgressReports on schobj.ExamSchId equals repobj.SchId
                                 where obj.ExamId == ExamId
                                 select repobj.StuMapId).Distinct().ToList();
                foreach (var CurStu in StuMapIds)
                {
                    var StuInfo = (from obj in context.StuStdAcdYearMaps
                                   join Stuobj in context.StudentInfos on obj.StuId equals Stuobj.StuId
                                   join StnObj in context.Standards on obj.StnId equals StnObj.StdId
                                   where obj.MapId==CurStu
                                   select new { Stuobj.RegdNo, Stuobj.FName, StnObj.StdName }).FirstOrDefault();

                    IndividualProgressReportDTO dto = new IndividualProgressReportDTO();
                    CWdto.lst.Add(dto);
                    dto.RegdNo = StuInfo.RegdNo;
                    dto.StuName = StuInfo.FName;
                    dto.StnName = StuInfo.StdName;

                    foreach (var sch in ExamInfo)
                    {
                        IndividualProgressReportSubject sub = new IndividualProgressReportSubject();
                        sub.SubjectName = sch.SubjectName;
                        dto.Subjects.Add(sub);

                        var repinfo = (from obj in context.ExamProgressReports
                                       join hobj in context.ExamProgressReportHeads on obj.PRHeadId equals hobj.PRHId
                                       where obj.SchId == sch.ExamSchId && obj.StuMapId == CurStu
                                       select new { HeadName = hobj.HeadName, obj.Marks, hobj.MaxMarks, hobj.MinMarks }).ToList();
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

                }
                Success = true;
            }
            catch (Exception e)
            {

            }
            if (Success)
            {
                return Ok(CWdto);
            }
            else
            {
                return BadRequest("Service error");
            }
        }

    }
}
