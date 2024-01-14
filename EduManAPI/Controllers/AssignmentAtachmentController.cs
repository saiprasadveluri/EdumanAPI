using System;
using System.Collections.Generic;
using System.IO;
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
    public class AssignmentAtachmentController : ControllerBase
    {
        EduManDBContext context = null;
        public AssignmentAtachmentController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetAttchment(Guid AssnId)
        {
            bool Success = false;
            AssignmentAtachmentGetDTO dto = new AssignmentAtachmentGetDTO();

            try
            {
                var res = (from obj in context.Assignments
                           where obj.AssnId == AssnId select obj).FirstOrDefault();
                if (res != null)
                {
                    dto.MimeType = res.MimeType;
                    string GuidName = res.AssnFileName;
                    string FilePath = Directory.GetCurrentDirectory() + "/Assignments/" + GuidName;
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
