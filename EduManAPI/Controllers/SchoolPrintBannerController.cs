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
    
    [ApiController]
    [EnableCors("Policy1")]
    public class SchoolPrintBannerController : ControllerBase
    {
        EduManDBContext context = null;
        public SchoolPrintBannerController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        [Route("api/[controller]/{id}")]
        public IActionResult AddBanner(SchoolPrintBannerDTO dto,int id)
        {
            bool Success = false;
            try
            {
                switch (id)
                {
                    case 1:
                        {
                            var SetRec = (from obj in context.SchoolSettings
                                          where obj.OrgId == dto.OrgId
                                          select obj).FirstOrDefault();
                            string FileGuid = null;
                            //Record Exists for the school
                            if (SetRec != null)
                            {
                                FileGuid = SetRec.PrintLogoFile;
                                if (FileGuid != null)
                                {
                                    DeleteFile(FileGuid);
                                }
                                SaveBannerImage(dto.BannerContent, FileGuid);
                                Success = true;
                            }
                            else
                            {
                                FileGuid = Guid.NewGuid().ToString();
                                SaveBannerImage(dto.BannerContent, FileGuid);
                                SchoolSetting SS = new SchoolSetting();
                                SS.SSID = Guid.NewGuid();
                                SS.PrintLogoFile = FileGuid;
                                SS.OrgId = dto.OrgId;
                                context.SchoolSettings.Add(SS);
                                context.SaveChanges();
                                Success = true;
                            }
                            break;
                        }
                    case 2:
                        {
                            var SetRec = (from obj in context.SchoolSettings
                                          where obj.OrgId == dto.OrgId
                                          select obj).FirstOrDefault();
                            if (SetRec != null)
                            {
                                SetRec.CurAcdYear = dto.AcdYearId;
                                context.SaveChanges();
                                Success = true;
                            }
                            else
                            {   
                                SchoolSetting SS = new SchoolSetting();
                                SS.SSID = Guid.NewGuid();                                
                                SS.OrgId = dto.OrgId;
                                SS.CurAcdYear = dto.AcdYearId;
                                context.SchoolSettings.Add(SS);
                                context.SaveChanges();
                                Success = true;
                            }
                            break;
                        }
                }
                
            }
            catch (Exception exp)
            { 
            
            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }


        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult GetSettings(Guid OrgId)
        {
            bool Success = false;

            SchoolPrintBannerDTO dto = new SchoolPrintBannerDTO();

            try
            {
                 if (OrgId != null)
                {
                    var rec = (from obj in context.SchoolSettings
                           join orgObj in context.Organizations on obj.OrgId equals orgObj.OrgId
                           join aobj in context.AcdYears on obj.CurAcdYear equals aobj.AcdId
                           where obj.OrgId == OrgId
                           select new {obj.PrintLogoFile,obj.CurAcdYear,aobj.AcdText}).FirstOrDefault();
                    if (rec != null)
                    {
                        dto.BannerContent = GetBannerImage(rec.PrintLogoFile);
                        dto.AcdYearId = rec.CurAcdYear.Value;
                        dto.AcdYearText = rec.AcdText;
                        Success = true;
                    }
                }                
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
                return BadRequest("error");
            }
        }

        private void DeleteFile(string GuidFilenName)
        {
            var FilePath = Directory.GetCurrentDirectory() + "/SchoolBanners/" + GuidFilenName;
            if(System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
        }

        private void SaveBannerImage(string FileBase64Content, string GuidFilenName)
        {
            var FilePath = Directory.GetCurrentDirectory() + "/SchoolBanners/" + GuidFilenName;
            byte[] ImageBytes = Convert.FromBase64String(FileBase64Content);
            FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            fs.Write(ImageBytes, 0, ImageBytes.Length);//TODO: Check for File Length check
            fs.Close();
        }
        private string GetBannerImage(string GuidFilenName)
        {
            String contentBase64 = String.Empty;
            try
            {
                var FilePath = Directory.GetCurrentDirectory() + "/SchoolBanners/" + GuidFilenName;
                //Save the Image File
                //byte[] ImageBytes = Convert.FromBase64String(Base64String);
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                byte[] FileBytes = new byte[fs.Length];
                fs.Read(FileBytes, 0, (int)fs.Length);
                fs.Close();
                contentBase64 = Convert.ToBase64String(FileBytes);
            }
            catch (Exception exp)
            {

            }
            return contentBase64;
        }

    }
}
