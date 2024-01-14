using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;

namespace EduManAPI.Infra
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly AppRoleEnum[] ArrRoleVals;
        
        public CustomAuthorizeAttribute(params AppRoleEnum[] ReqRoleVals)
        {
            ArrRoleVals = ReqRoleVals;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //context.HttpContext.Request.Headers.TryGetValue("ReqOrg", out StringValues ReqOrgLst);
            ////context.HttpContext.Request.Headers.TryGetValue("ReqRole", out StringValues ReqRoleList);
            //if(ReqOrgLst.Count==0)
            //{
            //    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            //    return;
            //}
            //Guid ReqOrg = Guid.Parse(ReqOrgLst[0]);
            ////Guid ReqRole = Guid.Parse(ReqRoleList[0]);

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }
            if (!user.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            //if(!user.HasClaim(c => c.Type == "OrgId"))
            //{
            //    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            //    return;
            //}


            //Check for condition
            int CurRoleValue = int.Parse(user.FindFirst(c => c.Type == ClaimTypes.Role).Value);
            if(CurRoleValue!=(int)AppRoleEnum.SITE_ADMIN)
            {
                if (!VerifyRole(CurRoleValue))
                {
                    var res = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                    context.Result = res;
                    return;
                }
            }            
        }

        private bool VerifyRole(int CurRoleVal)
        {
            foreach(var role in ArrRoleVals)
            {
                if((int)role== CurRoleVal)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
