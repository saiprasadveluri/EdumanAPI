using System;
using System.Collections.Generic;
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
    public class AssignmentSubmissionController : ControllerBase
    {
        EduManDBContext context = null;
        public AssignmentSubmissionController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetSubmissions(Guid AssmId)
        {
            List<AssignmentSubmissionDTO> lst = new List<AssignmentSubmissionDTO>();
            bool Success = false;
            try
            {
                lst = (from obj in context.AssignmentSubmissions
                       join hobj in context.Assignments on obj.AssnId equals hobj.AssnId
                       join StumapObj in context.StuStdAcdYearMaps on obj.StuMapId equals StumapObj.MapId
                       join Stnmapobj in context.StdSubMaps on hobj.MapId equals Stnmapobj.MapId
                       join Stnobj in context.Standards on Stnmapobj.StdId equals Stnobj.StdId
                       join subobj in context.Subjects on Stnmapobj.SubId equals subobj.SubId
                       join stuobj in context.StudentInfos on StumapObj.StuId equals stuobj.StuId
                       where obj.AssnId == AssmId
                       select new AssignmentSubmissionDTO()
                       {
                           AssmSubId=obj.AssnSubId,
                           SubDate = obj.SubDate,
                           StudentName = Stnobj.StdName,
                           SubName = subobj.SubjectName,
                           StnName = stuobj.FName,
                           RegdNo = stuobj.RegdNo
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
