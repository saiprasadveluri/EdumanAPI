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
    public class LoggedinStudentDetailsController : ControllerBase
    {
        EduManDBContext context = null;
        public LoggedinStudentDetailsController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetDetails(Guid LoginID, Guid AcdYearId)
        {
            bool Success = false;
            StudentDetailsDTO dto = new StudentDetailsDTO();
            try
            {
                dto = (from uiobj in context.UserInfos
                       join StuObj in context.StudentInfos on uiobj.ID equals StuObj.LoginUID
                       join mapobj in context.StuStdAcdYearMaps on StuObj.StuId equals mapobj.StuId
                       join StnObj in context.Standards on mapobj.StnId equals StnObj.StdId
                       where uiobj.ID == LoginID && mapobj.AcYearId == AcdYearId
                       select new StudentDetailsDTO
                       { 
                         ID=uiobj.ID,
                         Name=StuObj.FName,
                         MapId=mapobj.MapId,
                         StnID=StnObj.StdId,
                         StandardName= StnObj.StdName
                       }).FirstOrDefault();
                Success = true;
            }
            catch (Exception exp)
            { 
            
            }
            if (Success)
                return Ok(dto);
            else
                return BadRequest("Error");
        }

    }
}
