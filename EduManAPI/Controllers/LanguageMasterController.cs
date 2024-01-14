using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageMasterController : ControllerBase
    {
        EduManDBContext context = null;
        public LanguageMasterController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]

        public ActionResult GetAll(Guid OrgId)
        {
            bool Success = false;
            string Msg = "";
            List<LanguageMasterDTO> LangLst = new List<LanguageMasterDTO>();
            try
            {
                LangLst = context.LanguageMasters.Where(l=>l.OrgId==OrgId).Select(s => new LanguageMasterDTO()
                {
                    LangId= s.LangId,
                    LanguageName=s.LanguageName
                }).ToList();
                Success = true;
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
            }
            if (Success == true)
            {
                return Ok(LangLst);
            }
            else
            {
                return BadRequest(Msg);
            }
        }

        [HttpPost]
        public ActionResult Add(LanguageMasterDTO dto)
        {
            bool Success = false;

            try
            {
                LanguageMaster ay = new LanguageMaster();
                ay.LangId = Guid.NewGuid();
                ay.LanguageName = dto.LanguageName;
                ay.OrgId = dto.OrgId;
                context.LanguageMasters.Add(ay);
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
        public ActionResult Edit(LanguageMasterDTO dto)
        {
            bool Success = false;
            try
            {
                LanguageMaster enq = context.LanguageMasters.Where(e => e.LangId == dto.LangId).FirstOrDefault();
                if (enq != null)
                {
                    enq.LanguageName= dto.LanguageName;                   
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
        public IActionResult Delete(string chks)
        {
            bool success = false;
           
            string[] lst = chks.Split(',');

            List<LanguageMaster> CurRecs = null;
            try
            {
                List<string> OrIds = lst.ToList();
                CurRecs = (context.LanguageMasters.Where(s => OrIds.Contains(s.LangId.ToString()) == true).Select(s => s)).ToList();
                if (CurRecs != null)
                {
                    context.LanguageMasters.RemoveRange(CurRecs);
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
