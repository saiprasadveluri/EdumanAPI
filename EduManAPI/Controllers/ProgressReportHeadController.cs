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
    public class ProgressReportHeadController : ControllerBase
    {
        EduManDBContext context = null;
        public ProgressReportHeadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetHeads(Guid ExamId)
        {
            List<ProgressReportHeadDTO> dto = new List<ProgressReportHeadDTO>();
            bool Success = false;
            try
            {
                dto = (from obj in context.ExamProgressReportHeads
                           where obj.ExamId == ExamId
                           select new ProgressReportHeadDTO() {
                             PRHId=obj.PRHId,
                             ExamId = obj.ExamId,
                             MaxMarks =obj.MaxMarks,
                             MinMarks=obj.MinMarks,
                             HeadName = obj.HeadName
                           }).ToList();
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

        [HttpPut]
        public ActionResult UpdateHead(ProgressReportHeadDTO dto)
        {
            bool Success = false;
            try
            {
                var Cur = (from obj in context.ExamProgressReportHeads
                       where obj.PRHId == dto.PRHId
                       select obj
                       ).FirstOrDefault();
                Cur.MaxMarks = dto.MaxMarks;
                Cur.MinMarks = dto.MinMarks;
                Cur.HeadName = dto.HeadName;
                context.SaveChanges();
                Success = true;
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

        [HttpPost]
        public ActionResult AddHead(ProgressReportHeadDTO dto)
        {
            bool Success = false;

            try
            {
                ExamProgressReportHead h = new ExamProgressReportHead();
                h.PRHId = Guid.NewGuid();
                h.HeadName = dto.HeadName;
                h.ExamId = dto.ExamId;
                h.MaxMarks = dto.MaxMarks;
                h.MinMarks = dto.MinMarks;
                context.ExamProgressReportHeads.Add(h);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success == true)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public IActionResult DeletePRHeads(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<ExamProgressReportHead> CurHeads = null;
            try
            {
                List<string> OrIds = lst.ToList();
                CurHeads = (context.ExamProgressReportHeads.Where(s => OrIds.Contains(s.PRHId.ToString()) == true).Select(s => s)).ToList();
                if (CurHeads != null)
                {
                    context.ExamProgressReportHeads.RemoveRange(CurHeads);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
