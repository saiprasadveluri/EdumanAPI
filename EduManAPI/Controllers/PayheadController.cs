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
    public class PayheadController : ControllerBase
    {
        EduManDBContext context = null;
        public PayheadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult GetPayheads(Guid OrgId)
        {
            bool Success = false;
            List<PayheadDTO> Lst = new List<PayheadDTO>();
            try
            {
                Lst = (from obj in context.Payheads
                       where obj.OrgId == OrgId
                       orderby obj.Type
                       select new PayheadDTO() {
                           PHID = obj.PHID,
                           Name = obj.Name,
                           HeadType = obj.Type
                       }).ToList();
                Success = true;
            }
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok(Lst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult AddHead(PayheadDTO dto)
        {
            bool Success = false;
            try
            {
                Payhead ph = new Payhead();
                ph.PHID = Guid.NewGuid();
                ph.OrgId = dto.OrgId;
                ph.Name = dto.Name;
                ph.Type = dto.HeadType;
                context.Payheads.Add(ph);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception exp)
            { 
                
            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");

            }
        }

        [HttpPut]
        public ActionResult EditHead(PayheadDTO dto)
        {
            bool Success = false;
            try
            {
                Payhead ph = context.Payheads.Where(h => h.PHID == dto.PHID).FirstOrDefault();
                if (ph != null)
                {
                    ph.Name = dto.Name;
                    ph.Type = dto.HeadType;
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");

            }
        }

        [HttpDelete]
        public IActionResult DeletePayhead(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Payhead> CurPhs = null;
            try
            {
                List<string> OrIds = lst.ToList();
                CurPhs = (context.Payheads.Where(s => OrIds.Contains(s.PHID.ToString()) == true).Select(s => s)).ToList();
                if (CurPhs != null)
                {
                    context.Payheads.RemoveRange(CurPhs);
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
