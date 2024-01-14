using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentLanguageMapController : ControllerBase
    {
        EduManDBContext context = null;
        public StudentLanguageMapController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public IActionResult GetMaps(Guid StuStnAcdMapId)
        {
            var Res = (from obj in context.studentLangMaps
                       join lobj in context.LanguageMasters on obj.LanguageId equals lobj.LangId
                       where obj.StudentStnAcdMapId == StuStnAcdMapId
                       select new StudentLangMapDTO()
                       {
                           MapId = obj.MapId,
                           LangOrdinal = obj.LangOrdinal,
                           LanguageId = obj.LanguageId,
                           LanguageName=lobj.LanguageName
                       }).ToList();
            return Ok(Res);
        }

        [HttpPost]
        public IActionResult AddMap(AddStudentLanguageMapDTO inp )
        {
            StudentLangMap StuLangObj = new StudentLangMap()
            {
                MapId = Guid.NewGuid(),
                StudentStnAcdMapId = inp.StuMapId,
                LanguageId = inp.LanguageId,
                LangOrdinal = inp.LangOrdinal
            };
            context.studentLangMaps.Add(StuLangObj);
            context.SaveChanges();
            return new JsonResult("Success");
        }
        [HttpDelete]
        public IActionResult RemoveMap(Guid chks)
        {
            var CurRec=context.studentLangMaps.Where(l=>l.MapId == chks).FirstOrDefault();    
            if(CurRec != null)
            {
                context.studentLangMaps.Remove(CurRec);
                context.SaveChanges();
                return new JsonResult("Success");
            }
            return BadRequest("error");
                        
        }
    }
}
