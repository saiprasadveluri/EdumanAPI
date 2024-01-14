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
    public class AssnSubmissionDownloadController : ControllerBase
    {
        EduManDBContext context = null;
        public AssnSubmissionDownloadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult DownloadSubmission(Guid SubId)
        {
            bool Success = false;
            string FileBase64Str = string.Empty;
            AssnSubmissionDownloadDTO dto = new AssnSubmissionDownloadDTO();
            try
            {
                var res = (from obj in context.AssignmentSubmissions
                                where obj.AssnSubId == SubId
                                select new { obj.AssnSubPath,obj.MimeType }).FirstOrDefault();
                if (res != null)
                {
                    var ret = GetFileBase64String(res.AssnSubPath);
                    dto.FileBase64Str = ret.Item1;
                    dto.MimeType = res.MimeType;
                    Success = ret.Item2;
                }
            }
            catch (Exception exp)
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
        private Tuple<string, bool> GetFileBase64String(string flGuid)
        {
            string Str = "";
            bool Success = false;
            try
            {
                var FilePath = Directory.GetCurrentDirectory() + "/AssignmentSubmissions/" + flGuid;
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                byte[] ArrBytes = new byte[fs.Length];
                fs.Read(ArrBytes, 0, (int)fs.Length);
                fs.Close();
                Str = Convert.ToBase64String(ArrBytes);
                Success = true;
            }
            catch (Exception ex)
            {
                //Utility.LogToFile("From GetImageBase64String:" + ex.Message);
            }
            return Tuple.Create(Str, Success);
        }

    }
}
