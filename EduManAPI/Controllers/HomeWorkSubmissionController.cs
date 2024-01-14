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
    public class HomeWorkSubmissionController : ControllerBase
    {
        EduManDBContext context = null;
        public HomeWorkSubmissionController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddSubmission(HomeWorkSubmissionAddDTO dto)
        {
            bool Success = false;
            try
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        string flGuid = Guid.NewGuid().ToString();
                        //Save Data to DB...
                        HomeWorkSubmission hs = new HomeWorkSubmission();
                        hs.HWSubId = Guid.NewGuid();
                        hs.SubDate = DateTime.Now;
                        hs.HWId = dto.HWId;
                        hs.StuMapId = dto.StuMapId;
                        hs.HWSubPath = flGuid;
                        context.HomeWorkSubmissions.Add(hs);
                        context.SaveChanges();

                        //Save the FileContent to file...                        
                        var FilePath = Directory.GetCurrentDirectory() + "/HomeWork/" + flGuid;
                        byte[] FileBytes = Convert.FromBase64String(dto.FileContentBase64);
                        FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(FileBytes, 0, FileBytes.Length);//TODO: Check for File Length check
                        fs.Close();
                        trans.Commit();
                        Success = true;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
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
        public ActionResult GetSubmissions(Guid HWId)
        {
            List<HomeWorkSubmissionDTO> lst = new List<HomeWorkSubmissionDTO>();
            bool Success = false;
            try
            {
                lst = (from obj in context.HomeWorkSubmissions
                      join hobj in context.HomeWorks on obj.HWId equals hobj.HWId
                      join StumapObj in context.StuStdAcdYearMaps on obj.StuMapId equals StumapObj.MapId
                      join Stnmapobj in context.StdSubMaps on hobj.MapId equals Stnmapobj.MapId
                      join Stnobj in context.Standards on Stnmapobj.StdId equals Stnobj.StdId
                      join subobj in context.Subjects on Stnmapobj.SubId equals subobj.SubId
                      join stuobj in context.StudentInfos on StumapObj.StuId equals stuobj.StuId
                      where obj.HWId == HWId
                      select new HomeWorkSubmissionDTO() { 
                      HWSubId=obj.HWSubId,
                      SubDate=obj.SubDate,
                      StudentName= Stnobj.StdName,
                      SubName=subobj.SubjectName,
                      StnName = stuobj.FName,
                      RegdNo= stuobj.RegdNo
                      }).ToList();
                Success = true;
            }
            catch (Exception exp)
            { 
            
            }
            if (Success)
            {
                return Ok(lst);
            }
            else
            {
                return BadRequest("Service error");
            }
        }
    }
}
