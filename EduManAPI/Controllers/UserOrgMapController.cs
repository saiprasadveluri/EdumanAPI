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
    public class UserOrgMapController : ControllerBase
    {
        EduManDBContext context = null;
        public UserOrgMapController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpDelete]
        public IActionResult DeleteMap(Guid MId)
        {
            bool Success = false;
            try
            {
                var curobj = context.UserOrgMaps.Where(m => m.MapId == MId).FirstOrDefault();
                context.UserOrgMaps.Remove(curobj);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
            { 
            
            }
            if (Success == true)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }
        [HttpGet]
        [Route("{UserId}")]
        public IActionResult GetMaps(Guid UserId)
        {
            bool Success = false;
            List<UserOrgMapDTO> OrgMapLst = new List<UserOrgMapDTO>();
            try
            {
                OrgMapLst = (from mobj in context.UserOrgMaps
                          join rm in context.Roles on mobj.RoleId equals rm.RoleID
                          join oi in context.Organizations on mobj.OrgId equals oi.OrgId
                          where mobj.UserId == UserId
                          select new UserOrgMapDTO() { MapId = mobj.MapId,
                                                       RoleId= rm.RoleID,
                                                        RoleName = rm.RoleName,
                                                        OrgId= oi.OrgId,
                                                        OrgName = oi.OrgName}).ToList();
                Success = true;
            }
            catch (Exception ex)
            { 
            
            }
            if (Success == true)
            {
                return Ok(OrgMapLst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public IActionResult CrearteMap(UserOrgMapDTO inp)
        {
            bool Success = false;
            try
            {
                UserOrgMap m = new UserOrgMap();
                m.MapId = Guid.NewGuid();

                m.OrgId = inp.OrgId;
                m.UserId = inp.UserId;
                m.RoleId = inp.RoleId;
                context.UserOrgMaps.Add(m);
                context.SaveChanges();
                inp.MapId = m.MapId;
                Success = true;
            }
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok(inp);
            }
            else {
                return BadRequest("Error");
            }
        }
        }
}
