using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class UserInfoController : ControllerBase
    {
        EduManDBContext context = null;
        public UserInfoController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        [Route("{orgId}")]
        public IActionResult GetUsers(Guid? orgId)
        {
            bool Success = false;
            List<UserInfoDTO> UList = new List<UserInfoDTO>();
            try
            {
                UList = (from mobj in context.UserOrgMaps
                         join uobj in context.UserInfos on mobj.UserId equals uobj.ID
                         join robj in context.Roles on mobj.RoleId equals robj.RoleID
                         where mobj.OrgId == orgId
                         select new UserInfoDTO()
                         {
                             Id = uobj.ID,
                             OrgId = mobj.OrgId,
                             MapId = mobj.MapId,
                             Name = uobj.Name,
                             RoleVal = robj.RoleVal,
                             RoleName = robj.RoleName,
                             Emailid = uobj.Emailid,
                             MobNo = uobj.MobNo,
                             UserId = uobj.UserID,
                             password = uobj.Password
                         }).ToList();
                Success = true;
            }
            catch (Exception ex)
            { 
            
            }
            if (Success == true)
            {
                return Ok(UList);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{Orgid}/{id}")]
        public IActionResult GetUsers(Guid? Orgid,Guid? Id)
        {
            bool Success = false;
            List<UserInfoDTO> UList = new List<UserInfoDTO>();
            try
            {
                if (Id.HasValue)
                {
                    UList = context.UserInfos.Where(u => u.ID == Id).Select(u => new UserInfoDTO()
                    {
                        Id = u.ID,
                        Name = u.Name,
                        UserId = u.UserID,
                        MobNo = u.MobNo,
                        Emailid = u.Emailid
                    }).ToList();
                }
                else
                {
                    UList = context.UserInfos.Select(u => new UserInfoDTO()
                    {
                        Id = u.ID,
                        Name = u.Name,
                        UserId = u.UserID,
                        MobNo = u.MobNo,
                        Emailid = u.Emailid
                    }).ToList();
                }
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success == true)
            {
                return Ok(UList);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        /*public IActionResult DeleteUsers(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<UserInfo> CurUsrs = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurUsrs = (context.UserInfos.Where(s => OrIds.Contains(s.ID.ToString()) == true).Select(s => s)).ToList();
                if (CurUsrs != null)
                {
                    context.UserInfos.RemoveRange(CurUsrs);
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
        }*/
        public IActionResult DeleteUser(Guid recId)
        {
            bool success = false;
            try
            {
                var CurMap = context.UserOrgMaps.Where(m => m.MapId == recId).FirstOrDefault();
                if (CurMap != null)
                {
                    var UserId = CurMap.UserId;
                    var CurUser= context.UserInfos.Where(u=>u.ID==UserId).FirstOrDefault();
                    if(CurUser != null)
                    {
                        context.UserOrgMaps.Remove(CurMap);
                        context.UserInfos.Remove(CurUser);
                        context.SaveChanges();
                        success = true;
                    }
                }                
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

            return new JsonResult("Success");
            //return Ok("Success");
            
        }

        [HttpPut]
        public IActionResult EditUser(UserInfoDTO input)
        {
            bool Success = false;
            try
            {
                UserInfo ui = context.UserInfos.Where(u => u.ID == input.Id).FirstOrDefault();
                if (ui != null)
                {
                    ui.Name = input.Name;
                    ui.MobNo = input.MobNo;
                    ui.Emailid = input.Emailid;
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception ex)
            { 
            
            }
            if (Success == true)
            {
                return new JsonResult("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }
       
        [HttpPost]
        public IActionResult CrearteUser(UserInfoDTO input)
        {
            bool Success = false;
            string errMsg = "";
            UserInfo uinfo = new UserInfo();
            try
            {
                Guid CurRoleId=context.Roles.Where(r=>r.RoleVal==input.RoleVal).Select(r=>r.RoleID).FirstOrDefault();
                if (CurRoleId != Guid.Empty)
                {
                    using (var dbTrans = context.Database.BeginTransaction())
                    {
                        uinfo.ID = Guid.NewGuid();

                        uinfo.Name = input.Name;
                        uinfo.UserID = input.UserId;
                        uinfo.Password = "ChangeMe";//input.password;
                        uinfo.MobNo = input.MobNo;
                        uinfo.Emailid = input.Emailid;
                        context.UserInfos.Add(uinfo);
                        context.SaveChanges();
                        input.Id = uinfo.ID;

                        //Add User Org Map.
                        UserOrgMap m = new UserOrgMap();
                        m.MapId = Guid.NewGuid();
                        m.OrgId = input.OrgId.Value;
                        m.UserId = uinfo.ID;
                        m.RoleId = CurRoleId;
                        context.UserOrgMaps.Add(m);
                        context.SaveChanges();
                        dbTrans.Commit();
                        Success = true;
                    }
                }
                
                    
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            if (Success)
            {
                return Ok(input);
            }
            else
            {
                return BadRequest(errMsg);
            }
        }
    }
}