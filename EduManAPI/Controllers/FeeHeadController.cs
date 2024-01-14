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
    public class FeeHeadController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeHeadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddFeeHeads(FeeHeadMasterDTO dto)
        {
            bool Success = false;
            try
            {
                FeeHeadMaster fh = new FeeHeadMaster();
                fh.FeeHeadId = Guid.NewGuid();
                fh.OrgId = dto.OrgId;
                fh.FeeHeadName = dto.FeeHeadName;
                fh.Terms = dto.Terms;
                fh.FeeType = dto.FeeType;
                context.FeeHeadMasters.Add(fh);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception exp)
            { 
            }
            if (Success)
                return new JsonResult("Succeess");
            else
                return BadRequest("Service Error");
        }

        [HttpDelete]
        public IActionResult DeleteFeeheads(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<FeeHeadMaster> CurFs = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurFs = (context.FeeHeadMasters.Where(s => OrIds.Contains(s.FeeHeadId.ToString()) == true).Select(s => s)).ToList();
                if (CurFs != null)
                {
                    context.FeeHeadMasters.RemoveRange(CurFs);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (success)
            {
                return new JsonResult("Succeess");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpGet]
        public ActionResult GetFeeHeads(Guid? HeadId,Guid? OrgId)
        {
            List<FeeHeadMasterDTO> dto = new List<FeeHeadMasterDTO>();
            bool Success = false;
            try
            {
                if (HeadId.HasValue == false)
                {
                    dto = (from hobj in context.FeeHeadMasters
                           where hobj.OrgId == OrgId
                           select new FeeHeadMasterDTO()
                           {
                               FeeHeadId = hobj.FeeHeadId,
                               FeeHeadName = hobj.FeeHeadName,
                               Terms = hobj.Terms,
                               FeeType = hobj.FeeType
                           }).ToList();
                }
                else
                {
                    dto = (from hobj in context.FeeHeadMasters
                           where /*hobj.OrgId == OrgId.Value &&*/ hobj.FeeHeadId == HeadId.Value
                           select new FeeHeadMasterDTO()
                           {
                               FeeHeadId = hobj.FeeHeadId,
                               FeeHeadName = hobj.FeeHeadName,
                               Terms = hobj.Terms,
                               FeeType = hobj.FeeType
                           }).ToList();
                }
                Success = true;
            }
            catch (Exception exp)
            { 
            
            }
            if (Success == true)
                return Ok(dto);
            else
                return BadRequest("Service error");
        }
    }
}
