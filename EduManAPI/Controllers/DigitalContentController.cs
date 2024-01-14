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
    public class DigitalContentController : ControllerBase
    {
        EduManDBContext context = null;
        public DigitalContentController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetDigitalContent(Guid? DCId, Guid? OrgID,Guid? ChapID)
        {
            bool Success = false;
            List<DigitalContentDTO> lst = new List<DigitalContentDTO>();

            try
            {
                if (ChapID.HasValue)
                {
                    lst = (from obj in context.DigitalContents
                           join chap in context.SubChapeters on obj.ChapId equals chap.ChapId
                           join SSMap in context.StdSubMaps on chap.MapId equals SSMap.MapId
                           join SubObj in context.Subjects on SSMap.SubId equals SubObj.SubId
                           join StnObj in context.Standards on SSMap.StdId equals StnObj.StdId
                           where obj.ChapId == ChapID.Value
                           select new DigitalContentDTO()
                           {
                               DCId = obj.DCId,
                               Title = obj.Title,
                               Chapter = chap.ChapName,
                               Subject = SubObj.SubjectName,
                               Standard = StnObj.StdName,
                               DCType = obj.DCType,
                               Content = obj.ContentPath,
                               Status = obj.Status,
                               MimeType=obj.MimeType
                           }).ToList();
                    Success = true;
                }
                if (DCId.HasValue)
                {
                    var res = (from obj in context.DigitalContents
                           join chap in context.SubChapeters on obj.ChapId equals chap.ChapId
                           join SSMap in context.StdSubMaps on chap.MapId equals SSMap.MapId
                           join SubObj in context.Subjects on SSMap.SubId equals SubObj.SubId
                           join StnObj in context.Standards on SSMap.StdId equals StnObj.StdId
                           where obj.DCId == DCId.Value
                           select new DigitalContentDTO()
                           {
                               DCId= obj.DCId,
                               Title = obj.Title,
                               Chapter = chap.ChapName,
                               Subject = SubObj.SubjectName,
                               Standard = StnObj.StdName,
                               DCType = obj.DCType,
                               Content = obj.ContentPath,
                               Status = obj.Status,
                               MimeType = obj.MimeType
                           }).FirstOrDefault();
                    if (res != null)
                    {
                        //Read the content from the file...
                        if (res.DCType == 1 || res.DCType == 2)//Image and PDf content...
                        {
                            res.Content = RaedContentFromFile(res.Content);
                        }
                        lst.Add(res);
                        Success = true;
                    }
                }

                if(OrgID.HasValue)
                {
                    lst = (from obj in context.DigitalContents
                           join chap in context.SubChapeters on obj.ChapId equals chap.ChapId
                           join SSMap in context.StdSubMaps on chap.MapId equals SSMap.MapId
                           join SubObj in context.Subjects on SSMap.SubId equals SubObj.SubId
                           join StnObj in context.Standards on SSMap.StdId equals StnObj.StdId
                           where StnObj.OrgId==OrgID.Value
                           select new DigitalContentDTO()
                           {
                               DCId = obj.DCId,
                               Title = obj.Title,
                               Chapter = chap.ChapName,
                               Subject = SubObj.SubjectName,
                               Standard = StnObj.StdName,
                               DCType = obj.DCType,
                               Content = obj.ContentPath,
                               Status = obj.Status,
                               MimeType = obj.MimeType
                           }).ToList();
                }
                Success = true;
            }
            catch (Exception e)
            { 
            
            }

            if (Success)
            {
                return Ok(lst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult AddDigitalContent(DigitalContentAddDTO dto)
        {
            bool Success = false;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        DigitalContent content = new DigitalContent();
                        
                        string FileGuid = Guid.NewGuid().ToString();

                        content.DCId = Guid.NewGuid();
                        content.ChapId = dto.ChapId;
                        content.Title = dto.Title;
                        content.DCType = dto.DCType;
                        content.MimeType = dto.MimeType;
                        content.Status = dto.Status;
                        if (dto.DCType == 1 || dto.DCType == 2)//Image Or PDF:  Save the content as File
                        {
                            content.ContentPath = FileGuid;
                        }
                        else
                        {
                            content.ContentPath = dto.Content;
                        }
                        context.DigitalContents.Add(content);
                        context.SaveChanges();
                        bool FileSaveFlag = false;
                        //Now Add the File content to the disk.
                        if (dto.DCType == 1 || dto.DCType == 2)//Image Or PDF:  Save the content as File
                        {
                            FileSaveFlag = SaveContentToFile(FileGuid, dto.Content);
                        }
                        else
                        {
                            FileSaveFlag = true;
                        }
                        if (FileSaveFlag == true)
                        {
                            transaction.Commit();
                            Success = true;
                        }
                        else
                        {
                            transaction.Rollback();
                        }

                        }
                    catch (Exception exp)
                    { 
                    
                    }
                }           }
                catch (Exception e)
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

        [HttpDelete]
        public IActionResult DeleteDigContent(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<DigitalContent> CurDCs = null;
            try
            {
                List<string> Ids = lst.ToList();
                CurDCs = (context.DigitalContents.Where(s => Ids.Contains(s.DCId.ToString()) == true).Select(s => s)).ToList();
                if (CurDCs != null)
                {
                    context.DigitalContents.RemoveRange(CurDCs);
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

        private bool SaveContentToFile(string GuidFilenName, string Base64String)
        {
            bool Success = false;
            try
            {
                var FilePath = Directory.GetCurrentDirectory() + "/DigiContent/" + GuidFilenName;
                //Save the Image File
                byte[] ImageBytes = Convert.FromBase64String(Base64String);
                FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
                fs.Write(ImageBytes, 0, ImageBytes.Length);//TODO: Check for File Length check
                fs.Close();
                Success = true;
            }
            catch (Exception exp)
            {
                
            }
            return Success;
        }

        private string RaedContentFromFile(string GuidFilenName)
        {
            String contentBase64 = String.Empty;

            try
            {
                var FilePath = Directory.GetCurrentDirectory() + "/DigiContent/" + GuidFilenName;
                //Save the Image File
                //byte[] ImageBytes = Convert.FromBase64String(Base64String);
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                byte[] FileBytes = new byte[fs.Length];
                fs.Read(FileBytes, 0, (int)fs.Length);
                fs.Close();
                contentBase64=Convert.ToBase64String(FileBytes);
            }
            catch (Exception exp)
            {

            }
            return contentBase64;
        }
    }
}
