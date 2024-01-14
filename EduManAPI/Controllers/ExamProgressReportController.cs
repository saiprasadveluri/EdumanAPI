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
    public class ExamProgressReportController : ControllerBase
    {
        EduManDBContext context = null;
        public ExamProgressReportController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddSubjectReport(List<ExamProgressReportEntryDTO> dto)
        {
            bool Success = false;
            try
            {
                var CurSchId = dto.Select(s => s.SchId).FirstOrDefault();
                if (CurSchId != null)
                {
                    var ExistRecords = (from rec in context.ExamProgressReports
                                        where rec.SchId == CurSchId
                                        select rec).ToList();
                    if (ExistRecords != null)
                        context.ExamProgressReports.RemoveRange(ExistRecords);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var obj in dto)
                        {
                            ExamProgressReport rep = new ExamProgressReport();
                            rep.PRId = Guid.NewGuid();
                            rep.PRHeadId = obj.PRHeadId;
                            rep.SchId = obj.SchId;
                            rep.StuMapId = obj.StuMapId;
                            rep.Marks = obj.Marks;
                            rep.SubwiseRemarks = "";// obj.SubwiseRemarks;
                            context.ExamProgressReports.Add(rep);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        Success = true;
                    }
                    catch (Exception exp)
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Service error");
            }
        }
    }
}
