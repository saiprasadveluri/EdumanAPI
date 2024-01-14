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
    public class StuStnAcdMapController : ControllerBase
    {
        EduManDBContext context = null;
        public StuStnAcdMapController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
       public ActionResult GetStudentMapInfo([FromQuery] Guid MapId)
        {
            bool Success = false;
            StuStdMapDTO dto=null;
            try
            {
                dto = (from stmap in context.StuStdAcdYearMaps
                         join stuinfo in context.StudentInfos on stmap.StuId equals stuinfo.StuId
                         join stdobj in context.Standards on stmap.StnId equals stdobj.StdId
                         join AcdObj in context.AcdYears on stmap.AcYearId equals AcdObj.AcdId
                         where stmap.MapId== MapId
                         select new StuStdMapDTO()
                         {
                             MapId = stmap.MapId,
                             StudentId=stuinfo.StuId,
                             StandardId=stdobj.StdId,
                             StudentName = stuinfo.FName + "[" + stuinfo.LName + "]"
                         }).FirstOrDefault();
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
