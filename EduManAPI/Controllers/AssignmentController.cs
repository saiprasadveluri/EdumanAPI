using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class AssignmentController : ControllerBase
    {
        EduManDBContext context = null;
        public AssignmentController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public ActionResult GetAssignments(Guid StnId)
        {
            List<AssignmentGetListEntryDTO> dto = new List<AssignmentGetListEntryDTO>();
            bool Success = false;
            try
            {
                dto = (from obj in context.Assignments
                       join mapobj in context.StdSubMaps on obj.MapId equals mapobj.MapId
                       join stnobj in context.Standards on mapobj.StdId equals stnobj.StdId
                       join subobj in context.Subjects on mapobj.SubId equals subobj.SubId
                       where mapobj.StdId == StnId
                       select new AssignmentGetListEntryDTO()
                       {
                           AssnId = obj.AssnId,
                           AssnDate = obj.AssnDate.ToShortDateString(),
                           StnName = stnobj.StdName,
                           SubName = subobj.SubjectName,
                           Title = obj.Title
                       }).ToList();
                Success = true;
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

        [HttpPost]
        public ActionResult AddAssignment(AssignmentAddDTO dto)
        {
            bool Success = true;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string FileName = Guid.NewGuid().ToString();
                        Assignment assnObj = new Assignment();
                        assnObj.AssnId = Guid.NewGuid();
                        assnObj.MapId = dto.MapId;
                        assnObj.Title = dto.Title;
                        assnObj.AssnDate = dto.AssnDate;
                        assnObj.MimeType = dto.MimeType;
                        assnObj.AssnFileName = FileName;
                        context.Assignments.Add(assnObj);
                        context.SaveChanges();
                        //Save the File to folder...                        
                        string FilePath = Directory.GetCurrentDirectory() + "/Assignments/" + FileName;
                        byte[] FileBytes = Convert.FromBase64String(dto.FileBase64String);
                        FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(FileBytes, 0, FileBytes.Length);//TODO: Check for File Length check
                        fs.Close();
                        transaction.Commit();
                        Success = true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
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
    }
}
