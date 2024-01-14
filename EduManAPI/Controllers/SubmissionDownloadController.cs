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
    public class SubmissionDownloadController : ControllerBase
    {
        EduManDBContext context = null;
        public SubmissionDownloadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult DownloadSubmission(Guid SubId)
        {
            bool Success = false;
            string FileBase64Str = string.Empty;
            try
            {
                string fguid = (from obj in context.HomeWorkSubmissions
                             where obj.HWSubId == SubId
                             select obj.HWSubPath).FirstOrDefault();
                if (fguid != null)
                {
                    var ret = GetFileBase64String(fguid);
                    FileBase64Str = ret.Item1;
                    Success = ret.Item2;
                }
            }
            catch (Exception exp)
            { 
            
            }
            if (Success)
            {
                return Ok(FileBase64Str);
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
                var FilePath = Directory.GetCurrentDirectory() + "/HomeWork/" + flGuid;
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
