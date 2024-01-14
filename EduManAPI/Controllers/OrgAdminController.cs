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
    public class OrgAdminController : ControllerBase
    {
        EduManDBContext context = null;
        public OrgAdminController(EduManDBContext ctx)
        {
            context = ctx;
        }
        
        [HttpGet]
        public ActionResult GetAdmins(Guid? OrgId)
        {
            bool Success = false;
            List<OrgAdminDTO> dto = new List<OrgAdminDTO>(); 
            try
            {
                if (OrgId.HasValue)
                {
                    dto = (from mapobj in context.UserOrgMaps
                          join RMObj in context.Roles on mapobj.RoleId equals RMObj.RoleID
                           join UObj in context.UserInfos on mapobj.UserId equals UObj.ID
                          join OrgObj in context.Organizations on mapobj.OrgId equals OrgObj.OrgId
                          where mapobj.OrgId == OrgId.Value && RMObj.RoleVal == 2
                          select new OrgAdminDTO() { MapId=mapobj.MapId, OrgId = OrgObj.OrgId, OrgName = OrgObj.OrgName, AdminUserId = UObj.ID, AdmingName = UObj.Name }).ToList();
                }
                else
                {
                    dto = (from mapobj in context.UserOrgMaps
                           join RMObj in context.Roles on mapobj.RoleId equals RMObj.RoleID
                           join UObj in context.UserInfos on mapobj.UserId equals UObj.ID
                           join OrgObj in context.Organizations on mapobj.OrgId equals OrgObj.OrgId
                           where RMObj.RoleVal == 2
                           select new OrgAdminDTO() { MapId = mapobj.MapId, OrgId = OrgObj.OrgId, OrgName = OrgObj.OrgName, AdminUserId = UObj.ID, AdmingName = UObj.Name }).ToList();

                }
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
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult CreateAdmin(OrgAdminDTO dto)
        {
            bool Success = false;
            try
            {
                var temp = context.Roles.Where(r => r.RoleVal == 2).Select(r => r.RoleID).FirstOrDefault();
                UserOrgMap mp = new UserOrgMap();
                mp.MapId = Guid.NewGuid();
                mp.OrgId = dto.OrgId;
                mp.UserId = dto.AdminUserId;
                mp.RoleId = temp;
                context.UserOrgMaps.Add(mp);
                context.SaveChanges();
                Success = true;
                dto.MapId = mp.MapId;
            }
            catch (Exception exp)
            {

            }
            if (Success == true)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteMap(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<UserOrgMap> CurOrgMaps = null;
            try
            {
                List<string> MapIds = lst.ToList();// Select(s => long.Parse(s)).ToList();
                CurOrgMaps = (context.UserOrgMaps.Where(s => MapIds.Contains(s.MapId.ToString()) == true).Select(s => s)).ToList();
                if (CurOrgMaps != null)
                {
                    context.UserOrgMaps.RemoveRange(CurOrgMaps);
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
    }
}
