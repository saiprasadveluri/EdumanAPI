using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Newtonsoft.Json;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        EduManDBContext context = null;
        private IConfiguration _config;
        public LoginController(EduManDBContext ctx, IConfiguration config)
        {
            _config = config;
            context = ctx;
        }

        [HttpPost]
        public IActionResult VerifyLogin(LoginDTO li)
        {
            bool success = false;
            string ErrMsg = "";
            NewUserInfo uinfos = null ;
            try
            {
                uinfos = (from ui in context.UserInfos
                          where ui.UserID == li.UserID && ui.Password == li.Password
                          select new NewUserInfo()
                          { ID = ui.ID, Name = ui.Name,IsSiteAdmin=ui.IsSiteAdmin }).FirstOrDefault();                
                if (uinfos != null)
                {                    
                    if (li.GenToken == false)
                    { 
                        uinfos.UserOrgMapDTOs = GetUserOrgMap(uinfos.ID, context);                     
                    }                        
                    else
                    {
                        if(li.SelOrgId.HasValue)
                        {
                            var temp = (from ui in context.UserInfos
                                        join rm in context.UserOrgMaps on ui.ID equals rm.UserId
                                        join oobj in context.Organizations on rm.OrgId equals oobj.OrgId
                                        join roleobj in context.Roles on rm.RoleId equals roleobj.RoleID
                                        where ui.UserID == li.UserID && ui.Password == li.Password && rm.OrgId==li.SelOrgId
                                        select new NewUserInfo()
                                        { ID = ui.ID, Name = ui.Name, IsSiteAdmin = ui.IsSiteAdmin, RoleVal= roleobj.RoleVal, RoleString = roleobj.RoleName,OrgName= oobj.OrgName }).FirstOrDefault();
                            if(temp!=null)
                            {
                                uinfos.RoleString = temp.RoleString;
                                uinfos.OrgName = temp.OrgName;
                                uinfos.RoleVal= temp.RoleVal;
                            }
                            
                        }
                        var JwtTokenString = GenerateJSONWebToken(uinfos.ID,uinfos.IsSiteAdmin, li.SelOrgId, _config, context);
                        uinfos.JwtTokenString= JwtTokenString;
                    }
                    success = true;
                }
                else
                   ErrMsg = "No Match";
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                throw new AppException(101, ex.Message);
            }

            if (success)
            {   
                return Ok(uinfos);
            }
            else
            {
                throw new AppException(102, "Error In authentication");
            }
        }

      
        private static List<UserOrgMapDTO> GetUserOrgMap(Guid UserId,EduManDBContext ctx)
        {
            var res = (from obj in ctx.UserOrgMaps
                       join oobj in ctx.Organizations on obj.OrgId equals oobj.OrgId
                        join robj in ctx.Roles on obj.RoleId equals robj.RoleID
                        where obj.UserId == UserId
                        select new UserOrgMapDTO() { 
                        OrgId= obj.OrgId,
                        OrgName= oobj.OrgName,
                        RoleId=robj.RoleID,
                        RoleName=robj.RoleName,
                        RoleValue=robj.RoleVal,
                        UserId =UserId,
                        }).ToList();
            return res;
        }
        private static string GenerateJSONWebToken(Guid UserId,int? IsSiteAdmin, Guid? SelOrgId, IConfiguration _config, EduManDBContext ctx)
        {
            var AllowdClaims = new List<Claim>();
            try
            {
                if (IsSiteAdmin.HasValue)
                {
                    if(IsSiteAdmin.Value==1)
                    {
                        AllowdClaims.Add(new Claim(ClaimTypes.Role,((int)(AppRoleEnum.SITE_ADMIN)).ToString()));
                    }
                }
                else
                {
                    var UserRoleLst = GetUserOrgMap(UserId, ctx);
                    var SelMap = UserRoleLst.Where(ur => ur.UserId == UserId && ur.OrgId == SelOrgId.Value).FirstOrDefault();
                    if (SelMap != null)
                    {
                        AllowdClaims.Add(new Claim(ClaimTypes.Role, SelMap.RoleValue.ToString()));
                    }
                }
                
                if (AllowdClaims.Count>0)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                      _config["Jwt:Issuer"],
                      AllowdClaims,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    throw new AppException(104, "Error In creating Auth token");
                }  
            }
            catch (Exception ex)
            {
                throw new AppException(103, "Error In creating Auth token");
            }
        }

    }

    
}
