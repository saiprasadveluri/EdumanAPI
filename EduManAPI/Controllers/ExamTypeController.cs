using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class ExamTypeController : ControllerBase
    {
        EduManDBContext context = null;
        public ExamTypeController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetExamTypes(Guid OrgId)
        {
            List<ExamTypeDTO> Subs = new List<ExamTypeDTO>();
            bool Success = false;
            try
            {
                Subs = context.ExamTypes.Where(s => s.OrgId == OrgId).
                    Select(s => new ExamTypeDTO()
                    {
                        ExamTypeId=s.ExamTypeId,
                        OrgId = s.OrgId,
                        TypeText=s.TypeText
                    }).ToList();
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success)
            {
                return Ok(Subs);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult AddExamType(ExamTypeDTO dto)
        {
            bool Success = false;
            try
            {
                ExamType Exm = new ExamType();
                Exm.ExamTypeId = Guid.NewGuid();

                Exm.OrgId = dto.OrgId;
                Exm.TypeText = dto.TypeText;
                
                context.ExamTypes.Add(Exm);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
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

        [HttpDelete]
        public IActionResult DeleteExamType(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<ExamType> CurExms = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurExms = (context.ExamTypes.Where(s => OrIds.Contains(s.ExamTypeId.ToString()) == true).Select(s => s)).ToList();
                if (CurExms != null)
                {
                    context.ExamTypes.RemoveRange(CurExms);
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

        [HttpPut]
        public ActionResult EditExamType(ExamType dto)
        {
            bool Success = false;
            try
            {
                ExamType Exm = context.ExamTypes.Where(s => s.ExamTypeId == dto.ExamTypeId).FirstOrDefault();
                if (Exm != null)
                {
                    Exm.TypeText = dto.TypeText;
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception ex)
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
    }
}
