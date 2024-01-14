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
    public class StandardController : ControllerBase
    {
        EduManDBContext context = null;
        public StandardController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetStandards(Guid OrgId)
        {
            List<StandardDTO> Subs = new List<StandardDTO>();
            bool Success = false;
            try
            {
                Subs = context.Standards.Where(s => s.OrgId == OrgId).
                    Select(s => new StandardDTO()
                    {
                        StdId=s.StdId,
                        OrgId = s.OrgId,
                        StdName=s.StdName
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
        public ActionResult AddStandard(StandardDTO dto)
        {
            bool Success = false;
            try
            {
                Standard stn = new Standard();
                stn.StdId = Guid.NewGuid();
                
                stn.OrgId = dto.OrgId;
                stn.StdName = dto.StdName;
                context.Standards.Add(stn);
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
        public IActionResult DeleteStandards(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Standard> CurStds = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurStds = (context.Standards.Where(s => OrIds.Contains(s.StdId.ToString()) == true).Select(s => s)).ToList();
                if (CurStds != null)
                {
                    context.Standards.RemoveRange(CurStds);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (success)
            {
                return new JsonResult("Success"); ;
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        public ActionResult EditStandard(StandardDTO dto)
        {
            bool Success = false;
            try
            {
                Standard Std = context.Standards.Where(s=>s.StdId == dto.StdId).FirstOrDefault();
                Std.StdName = dto.StdName;
                Std.OrgId = dto.OrgId;
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

    }
}
