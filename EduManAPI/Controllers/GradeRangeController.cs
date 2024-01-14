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
    public class GradeRangeController : ControllerBase
    {
        EduManDBContext context = null;
        public GradeRangeController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetAllGrades(Guid OrgId)
        {
            bool Success = false;
            List<GradeRangeDTO> Lst = new List<GradeRangeDTO>();
            try
            {
                Lst = context.GradeRanges.Where(s=>s.OrgId==OrgId).Select(s => new GradeRangeDTO()
                {
                   GRID=s.GRID,
                   GradeText = s.GradeText,
                   MinMarks = s.MinMarks,
                   MaxMarks = s.MaxMarks,
                   GradePoint = s.GradePoint
                }).OrderBy(s=>s.GradePoint).ToList();
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success == true)
            {
                return Ok(Lst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult AddGrade(GradeRangeDTO dto)
        {
            bool Success = false;

            try
            {
                GradeRange gr = new GradeRange();
                gr.GRID = Guid.NewGuid();
                gr.OrgId = dto.OrgId;
                gr.MinMarks = dto.MinMarks;
                gr.MaxMarks = dto.MaxMarks;
                gr.GradeText = dto.GradeText;
                gr.GradePoint = dto.GradePoint;
                context.GradeRanges.Add(gr);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success == true)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        public ActionResult EditGrade(GradeRangeDTO dto)
        {
            bool Success = false;
            try
            {
                GradeRange enq = context.GradeRanges.Where(e => e.GRID == dto.GRID).FirstOrDefault();
                if (enq != null)
                {
                    enq.GradeText = dto.GradeText;
                    enq.MinMarks = dto.MinMarks;
                    enq.MaxMarks = dto.MaxMarks;
                    enq.GradePoint = dto.GradePoint;
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (Success == true)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteGrades(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<GradeRange> CurGrades = null;
            try
            {
                List<string> OrIds = lst.ToList();
                CurGrades = (context.GradeRanges.Where(s => OrIds.Contains(s.GRID.ToString()) == true).Select(s => s)).ToList();
                if (CurGrades != null)
                {
                    context.GradeRanges.RemoveRange(CurGrades);
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
