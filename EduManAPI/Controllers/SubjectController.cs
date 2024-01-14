using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    //[CustomAuthorizeAttribute((int)RoleEnum.OrgAdmin, (int)RoleEnum.SiteAdmin)]
    public class SubjectController : ControllerBase
    {
        EduManDBContext context = null;
        public SubjectController(EduManDBContext ctx)
        {
            context = ctx;
        }
        
        [HttpGet]
        public ActionResult GetSubjects(Guid OrgId)
        {
            List<SubjectDTO> Subs = new List<SubjectDTO>();
            bool Success = false;
            try
            {
                Subs = context.Subjects.Where(s => s.OrgId == OrgId).
                    Select(s => new SubjectDTO() {
                    SubId = s.SubId,
                    OrgId = s.OrgId,
                    SubjectName = s.SubjectName,
                    SubCode = s.SubCode,
                    IsLanguage=s.IsLanguage,
                    LangOrdinal=s.LangOrdinal
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
        public ActionResult AddSubject(SubjectDTO dto)
        {
            bool Success = false;
            try
            {
                Subject sub = new Subject();
                sub.SubId = Guid.NewGuid();

                sub.SubjectName = dto.SubjectName;
                sub.OrgId = dto.OrgId;
                sub.SubCode = dto.SubCode;
                sub.IsLanguage = dto.IsLanguage;
                sub.LangOrdinal = dto.LangOrdinal;

                context.Subjects.Add(sub);
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
        public IActionResult DeleteSubject(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Subject> CurSubs = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurSubs = (context.Subjects.Where(s => OrIds.Contains(s.SubId.ToString()) == true).Select(s => s)).ToList();
                if (CurSubs != null)
                {
                    context.Subjects.RemoveRange(CurSubs);
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

        [HttpPut]
        public ActionResult EditSubject(SubjectDTO dto)
        {
            bool Success = false;
            try
            {
                Subject sub = context.Subjects.Where(s=>s.SubId==dto.SubId).FirstOrDefault();
                sub.SubjectName = dto.SubjectName;
                sub.SubCode = dto.SubCode;
                sub.IsLanguage = dto.IsLanguage;
                sub.LangOrdinal = dto.LangOrdinal;
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
