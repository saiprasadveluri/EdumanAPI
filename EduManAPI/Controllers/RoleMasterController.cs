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
    public class RoleMasterController : ControllerBase
    {
        EduManDBContext context = null;
        public RoleMasterController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            List<RoleMasterDTO> Rls = new List<RoleMasterDTO>();
            bool Success = false;
            try
            {
                Rls = context.Roles.Select(s => new RoleMasterDTO()
                {
                    RoleID = s.RoleID,
                    RoleVal = s.RoleVal,
                    RoleName = s.RoleName
                }).ToList();
                Success = true;
            }
            catch (Exception exp)
            { 
                
            }
            if (Success == true)
            {
                return Ok(Rls);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
