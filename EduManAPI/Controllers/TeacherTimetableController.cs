using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class TeacherTimetableController : ControllerBase
    {
        EduManDBContext context = null;
        public TeacherTimetableController(EduManDBContext ctx)
        {
            context = ctx;
        }
        
        
        [HttpGet]
        public ActionResult GetTimeTable(Guid TeacherID)
        {
            bool Success = false;
            List<ViewTeacherTimeTableEntryDTO> dto = new List<ViewTeacherTimeTableEntryDTO>();
            try
            {
                dto = (from obj in context.TimeTables
                       join mapobj in context.StdSubMaps on obj.StnSubMapId equals mapobj.MapId
                       join sobj in context.Subjects on mapobj.SubId equals sobj.SubId
                       join stdObj in context.Standards on mapobj.StdId equals stdObj.StdId
                       where obj.TeacherId == TeacherID
                       select new ViewTeacherTimeTableEntryDTO() { 
                        WeekNo=obj.WeekNo,
                        HourNo= obj.HourNo,
                        StnName= stdObj.StdName,
                        SubName= sobj.SubjectName
                       }).ToList();
                Success = true;
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
                return BadRequest("Error");
            }
        }
    }
}