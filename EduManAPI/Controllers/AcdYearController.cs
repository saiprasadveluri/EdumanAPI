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
    public class AcdYearController : ControllerBase
    {
        EduManDBContext context = null;
        public AcdYearController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetAllAcdYears()
        {
            bool Success = false;
            string Msg = "";
            List<AcdYearDTO> AcdLst = new List<AcdYearDTO>();
            try
            {
                AcdLst = context.AcdYears.Select(s => new AcdYearDTO() {
                AcdId= s.AcdId,
                AcdText = s.AcdText
                }).ToList();
                Success = true;
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
            }
            if (Success == true)
            {
                return Ok(AcdLst);
            }
            else
            {
                return BadRequest(Msg);
            }
        }

        [HttpPost]
        public ActionResult AddAcdYear(AcdYearDTO dto)
        {
            bool Success = false;
            
            try
            {
                AcdYear ay = new AcdYear();
                ay.AcdId = Guid.NewGuid();
                ay.AcdText = dto.AcdText;
                context.AcdYears.Add(ay);
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
        public ActionResult EditAcdYear(AcdYearDTO dto)
        {
            bool Success = false;
            try
            {
                AcdYear enq = context.AcdYears.Where(e => e.AcdId==dto.AcdId).FirstOrDefault();
                if (enq != null)
                {
                    enq.AcdText = dto.AcdText;
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
        public IActionResult DeleteAcdyears(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<AcdYear> CurAcds = null;
            try
            {
                List<string> OrIds = lst.ToList();
                CurAcds = (context.AcdYears.Where(s => OrIds.Contains(s.AcdId.ToString()) == true).Select(s => s)).ToList();
                if (CurAcds != null)
                {
                    context.AcdYears.RemoveRange(CurAcds);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (success)
            {
                return new JsonResult("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }

    }
}
