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
    public class SelectElementDataController : ControllerBase
    {
        EduManDBContext context = null;
        public SelectElementDataController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetData(int Opt,Guid OrgId, Guid? AcdYear)
        {
            List<SelectElementDataDTO> lst = new List<SelectElementDataDTO>();
            bool Success = false;
            try
            {
                switch (Opt)
                {
                    case 1://Get All Students
                        lst = (from obj in context.StuStdAcdYearMaps
                               join Stuobj in context.StudentInfos on obj.StuId equals Stuobj.StuId
                               join Stnobj in context.Standards on obj.StnId equals Stnobj.StdId
                               join Uinfoobj in context.UserInfos on Stuobj.LoginUID equals Uinfoobj.ID
                               where Stnobj.OrgId == OrgId && obj.AcYearId== AcdYear
                               select new SelectElementDataDTO()
                               {
                                   ID = Uinfoobj.ID.ToString(),//obj.MapId.ToString(),
                                   name = Stuobj.FName + "[ " + Stnobj.StdName + " ]"
                               }).ToList();
                        break;
                    case 2:
                        lst = (from obj in context.Teachers
                               join Uinfoobj in context.UserInfos on obj.LoginUID equals Uinfoobj.ID
                               where obj.OrgId == OrgId
                               select new SelectElementDataDTO()
                               {
                                   ID = Uinfoobj.ID.ToString(),//obj.TeacherId.ToString(),
                                   name = obj.FName
                               }).ToList();

                        break;
                    case 3:
                        lst = (from obj in context.Standards
                               where obj.OrgId == OrgId
                               select new SelectElementDataDTO()
                               {
                                   ID = obj.StdId.ToString(),
                                   name = obj.StdName
                               }).ToList();
                        break;
                }
                Success = true;
            }
            catch (Exception e)
            { 
                
            }
            if (Success)
                return Ok(lst);
            else
                return BadRequest("Error");

        }
    }
}
