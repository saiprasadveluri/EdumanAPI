using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    
    public class OrganizationController : ControllerBase
    {
        EduManDBContext context = null;
        public OrganizationController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]        
        public ActionResult GetOrganization(Guid? Od,string OrgCode)
        {
            List<OrganizationDTO> Orgs = new List<OrganizationDTO>();
            bool success = false;
            try
            {
                if (Od.HasValue)
                {
                    Orgs = context.Organizations.Where(o => o.OrgId == Od.Value)
                        .Select(o => new OrganizationDTO() {OrgId = o.OrgId,
                                                            OrgName = o.OrgName,
                                                            OrgAddress=o.OrgAddress,
                                                            OrgPOC=o.OrgPOC,
                                                            OrgMobile=o.OrgMobile,
                                                            OrgEmail=o.OrgEmail,
                                                            OrgCode = o.OrgCode
                                                            }).ToList();
                }
                else if(OrgCode!=null && OrgCode.Length>0)
                {
                    Orgs = context.Organizations.Where(o => o.OrgCode == OrgCode)
                        .Select(o => new OrganizationDTO()
                        {
                            OrgId = o.OrgId,
                            OrgName = o.OrgName,
                            OrgAddress = o.OrgAddress,
                            OrgPOC = o.OrgPOC,
                            OrgMobile = o.OrgMobile,
                            OrgEmail = o.OrgEmail,
                            OrgCode=o.OrgCode
                        }).ToList();
                }
                else
                {
                    Orgs = context.Organizations
                        .Select(o => new OrganizationDTO()
                        {
                            OrgId = o.OrgId,
                            OrgName = o.OrgName,
                            OrgAddress = o.OrgAddress,
                            OrgPOC = o.OrgPOC,
                            OrgMobile = o.OrgMobile,
                            OrgEmail = o.OrgEmail,
                            OrgCode = o.OrgCode
                        }).ToList();
                    
                }
                success = true;
            }
            catch (Exception exp)
            { 
            
            }
            if (success == true)
            {
                return Ok(Orgs);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        public ActionResult UpdateOrganization(OrganizationDTO inp)
        {
            bool success = false;
            try
            {
                var CurOrg = context.Organizations.Where(o => o.OrgId == inp.OrgId).FirstOrDefault();
                if (CurOrg != null)
                {
                    CurOrg.OrgName = inp.OrgName;
                    CurOrg.OrgAddress = inp.OrgAddress;
                    CurOrg.OrgPOC = inp.OrgPOC;
                    CurOrg.OrgEmail = inp.OrgEmail;
                    CurOrg.OrgMobile = inp.OrgMobile;
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            { 
                
            }
            if (success == true)
            {
                return Ok(inp);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        [CustomAuthorize(AppRoleEnum.SITE_ADMIN)]
        public IActionResult DeleteOrganizations(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Organization> CurOrgs = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurOrgs = (context.Organizations.Where(s => OrIds.Contains(s.OrgId.ToString()) == true).Select(s => s)).ToList();
                if (CurOrgs != null)
                {
                    context.Organizations.RemoveRange(CurOrgs);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            { 
            
            }
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        [CustomAuthorize(AppRoleEnum.SITE_ADMIN)]
        public ActionResult CreateOrganization(OrganizationDTO inp)
        {
            bool success = false;
            try
            {
                Organization org = new Organization();
                org.OrgId = Guid.NewGuid();
                using (var dbTrans = context.Database.BeginTransaction())
                {
                    org.OrgName = inp.OrgName;
                    org.OrgAddress = inp.OrgAddress;
                    org.OrgPOC = inp.OrgPOC;
                    org.OrgMobile = inp.OrgMobile;
                    org.OrgEmail = inp.OrgEmail;
                    org.OrgCode = inp.OrgCode;
                    context.Organizations.Add(org);
                    context.SaveChanges();
                    inp.OrgId = org.OrgId;
                    success = true;
                    //Add Site Admins to the Newly created Organizations.
                    var SiteAdmins = (from obj in context.UserInfos
                                      where obj.IsSiteAdmin == 1
                                      select obj).ToList();
                    if(SiteAdmins.Count()>0)
                    {
                        //Add user to Map as SiteAdmin role.
                        for(int i=0;i<SiteAdmins.Count(); i++)
                        {
                            var SiteAdminRoleId = (from obj in context.Roles
                                                   where obj.RoleVal == (int)AppRoleEnum.SITE_ADMIN
                                                   select obj.RoleID).FirstOrDefault();
                            var OrgUserMapObj = new UserOrgMap()
                            {
                                MapId = Guid.NewGuid(),
                                OrgId = org.OrgId,
                                UserId = SiteAdmins[i].ID,
                                RoleId = SiteAdminRoleId
                            };
                            context.UserOrgMaps.Add(OrgUserMapObj);
                        }
                        context.SaveChanges();
                    }
                    dbTrans.Commit();
                }
                
            }
            catch (Exception exp)
            { 
            
            }
            if (success == true)
            {
                return Ok(inp);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
