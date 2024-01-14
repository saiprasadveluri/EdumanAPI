using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class BulkTeacherUploadController : ControllerBase
    {
        EduManDBContext context = null;
        public BulkTeacherUploadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult UploadData(List<AddTeacherDTO> dto)
        {
            bool Success = false;
            var RID = (from robj in context.Roles
                       where robj.RoleVal == (int)RoleEnum.Teacher
                       select robj.RoleID).FirstOrDefault();
            if (dto.Count < 0)
            {
                return BadRequest("Empty List");
            }
            var OrgId = dto[0].OrgId;
            var OrgCode = (from oobj in context.Organizations
                           where oobj.OrgId == OrgId
                           select oobj.OrgCode).FirstOrDefault();
            if(OrgCode==null)
            {
                return BadRequest("Error In OrgCode");
            }
            List<string> ErrorEmpNos = new List<string>();
            
                foreach (var tinfo in dto)
                {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {


                        //Add Login Info....
                        UserInfo uinfo = new UserInfo();
                        uinfo.ID = Guid.NewGuid();
                        uinfo.Name = tinfo.FName;
                        uinfo.MobNo = tinfo.MobileNo;
                        uinfo.Emailid = tinfo.EmpId;
                        uinfo.UserID = OrgCode+"_TEA" + tinfo.EmpId;
                        uinfo.Password = "changeme";
                        context.UserInfos.Add(uinfo);
                        context.SaveChanges();
                        //Update User-Org-map table...
                        UserOrgMap umap = new UserOrgMap();
                        umap.MapId = Guid.NewGuid();
                        umap.OrgId = tinfo.OrgId;
                        umap.RoleId = RID;
                        umap.UserId = uinfo.ID;
                        context.UserOrgMaps.Add(umap);
                        context.SaveChanges();


                        Teacher tch = new Teacher();
                        tch.TeacherId = Guid.NewGuid();
                        tch.OrgId = tinfo.OrgId;
                        tch.EmpId = tinfo.EmpId;
                        tch.FName = tinfo.FName;
                        tch.MName = tinfo.MName;
                        tch.LName = tinfo.LName;
                        tch.DOJoining = tinfo.DOJoining;
                        tch.Address = tinfo.Address;
                        tch.MobileNo = tinfo.MobileNo;
                        tch.FatherName = tinfo.FatherName;
                        tch.MotherName = tinfo.MotherName;
                        tch.Gender = tinfo.Gender;
                        tch.BllodGroup = tinfo.BllodGroup;
                        tch.TeacherType = tinfo.TeacherType;
                        tch.LoginUID = uinfo.ID;

                        tch.Status = 1;//Active
                        context.Teachers.Add(tch);
                        context.SaveChanges();
                        transaction.Commit();

                        Success = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ErrorEmpNos.Add(tinfo.EmpId);
                    }
                }
                }
            return Ok(ErrorEmpNos);
        }
    }
}
