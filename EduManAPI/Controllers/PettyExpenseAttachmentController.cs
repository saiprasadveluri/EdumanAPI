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
    public class PettyExpenseAttachmentController : ControllerBase
    {
        EduManDBContext context = null;
        public PettyExpenseAttachmentController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetAttchment(Guid BillId)
        {
            bool Success = false;
            BillAttachmentDTO dto = new BillAttachmentDTO();

            try
            {
                var res = (from obj in context.PettyExpnses
                           where obj.PEID == BillId
                           select obj).FirstOrDefault();
                if (res != null)
                {
                    dto.MimeType = res.ContentType;
                    string GuidName = res.FileGuid;
                    string FilePath = Directory.GetCurrentDirectory() + "/PettyExpenseBills/" + GuidName;
                    if (System.IO.File.Exists(FilePath))
                    {
                        FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                        byte[] ArrBytes = new byte[fs.Length];
                        fs.Read(ArrBytes, 0, (int)fs.Length);
                        dto.Base64Content = Convert.ToBase64String(ArrBytes);
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
