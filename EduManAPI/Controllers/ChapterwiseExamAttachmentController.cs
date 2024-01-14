using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class ChapterwiseExamAttachmentController : ControllerBase
    {
        EduManDBContext context = null;
        public ChapterwiseExamAttachmentController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetAttchment(Guid ExamId)
        {
            bool Success = false;
            AtachmentGetDTO dto = new AtachmentGetDTO();

            try
            {
                var res = (from obj in context.ChatpterwiseExams
                           where obj.ExamId==ExamId
                           select obj).FirstOrDefault();
                if (res != null)
                {
                    dto.MimeType = res.MimeType;
                    string GuidName = res.FileName;
                    string FilePath = Directory.GetCurrentDirectory() + "/ChapterExams/" + GuidName;
                    if (System.IO.File.Exists(FilePath))
                    {
                        FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                        byte[] ArrBytes = new byte[fs.Length];
                        fs.Read(ArrBytes, 0, (int)fs.Length);
                        dto.FileBase64String = Convert.ToBase64String(ArrBytes);
                        Success = true;
                    }
                }
            }
            catch
            {

            }
            if (Success)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Service error");
            }
        }
    }
}
