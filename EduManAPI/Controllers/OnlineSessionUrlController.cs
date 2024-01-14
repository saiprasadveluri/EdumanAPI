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
    public class OnlineSessionUrlController : ControllerBase
    {
        EduManDBContext context = null;
        public OnlineSessionUrlController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetSessionUrls(Guid? TeachId,Guid? OrgId)
        {
            List<OnlineSessionUrlDTO> SesLst = new List<OnlineSessionUrlDTO>();
            bool Success = false;
            try
            {
                if (TeachId.HasValue)
                {
                    SesLst = (from obj in context.OnlineSessionUrls
                               join tobj in context.Teachers on obj.TeacherId equals tobj.TeacherId
                               where obj.TeacherId == TeachId.Value
                               select new OnlineSessionUrlDTO() {
                                   OSUrlId = obj.OSUrlId,
                                   Title = obj.Title,
                                   TeacherId = obj.TeacherId,
                                   TeacherName = tobj.FName,
                                   SessionUrl= obj.SessionUrl
                               }).ToList();
                }
                else if(OrgId.HasValue)
                {
                    SesLst = (from obj in context.OnlineSessionUrls
                              join tobj in context.Teachers on obj.TeacherId equals tobj.TeacherId
                              where tobj.OrgId == OrgId.Value
                              select new OnlineSessionUrlDTO()
                              {
                                  OSUrlId = obj.OSUrlId,
                                  Title = obj.Title,
                                  TeacherId = obj.TeacherId,
                                  TeacherName = tobj.FName,
                                  SessionUrl = obj.SessionUrl
                              }).ToList();

                }
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success)
            {
                return Ok(SesLst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult AddSessionUrl(OnlineSessionUrlDTO dto)
        {
            bool Success = false;
            try
            {
                OnlineSessionUrl sesobj = new OnlineSessionUrl();

                sesobj.OSUrlId = Guid.NewGuid();
                sesobj.Title = dto.Title;
                sesobj.TeacherId = dto.TeacherId;
                sesobj.SessionUrl = dto.SessionUrl;

                context.OnlineSessionUrls.Add(sesobj);
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
        public IActionResult DeleteUrls(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<OnlineSessionUrl> CurUrls = null;
            try
            {
                List<string> Ids = lst.ToList();
                CurUrls = (context.OnlineSessionUrls.Where(s => Ids.Contains(s.OSUrlId.ToString()) == true).Select(s => s)).ToList();
                if (CurUrls != null)
                {
                    context.OnlineSessionUrls.RemoveRange(CurUrls);
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
