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
    public class BulkStudentUploadController : ControllerBase
    {
        EduManDBContext context = null;
        public BulkStudentUploadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult UploadData(BulkAddStudentInfoDTO dto)
        {
            bool Success = false;
            List<string> ErrorRegNos = new List<string>();
            try
            {
                var RID = (from robj in context.Roles
                           where robj.RoleVal == (int)RoleEnum.Student
                           select robj.RoleID).FirstOrDefault();


                foreach (var stuinfo in dto.StuLst)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //Add Login Info....
                            UserInfo uinfo = new UserInfo();
                            uinfo.ID = Guid.NewGuid();
                            uinfo.Name = stuinfo.FName;
                            uinfo.MobNo = stuinfo.FatherMobile;
                            uinfo.Emailid = stuinfo.ParentEmail;
                            uinfo.UserID = "STU" + stuinfo.RegdNo;
                            uinfo.Password = "changeme";
                            context.UserInfos.Add(uinfo);
                            context.SaveChanges();
                            //Update User-Org-map table...
                            UserOrgMap umap = new UserOrgMap();
                            umap.MapId = Guid.NewGuid();
                            umap.OrgId = stuinfo.OrgId;
                            umap.RoleId = RID;
                            umap.UserId = uinfo.ID;
                            context.UserOrgMaps.Add(umap);
                            context.SaveChanges();

                            //Add Student Info....
                            StudentInfo obj = new StudentInfo();
                            obj.StuId = Guid.NewGuid();
                            obj.RegdNo = stuinfo.RegdNo;
                            obj.FName = stuinfo.FName;
                            obj.MName = stuinfo.MName;
                            obj.LName = stuinfo.LName;
                            obj.DOBirth = stuinfo.DOBirth;
                            obj.DOAdmission = stuinfo.DOAdmission;
                            obj.ResAddress = stuinfo.ResAddress;
                            obj.FatherName = stuinfo.FatherName;
                            obj.MotherName = stuinfo.MotherName;
                            obj.FatherMobile = stuinfo.FatherMobile;
                            obj.ParentEmail = stuinfo.ParentEmail;
                            obj.Gender = stuinfo.Gender;
                            obj.BloodGroup = stuinfo.BloodGroup;
                            obj.AadharNo = stuinfo.AadharNo;
                            obj.Religion = stuinfo.Religion;
                            obj.Cast = stuinfo.Cast;
                            obj.SchoolAdmNo = stuinfo.SchoolAdmNo;
                            obj.IsActive = stuinfo.IsActive;
                            obj.LoginUID = uinfo.ID;

                            context.StudentInfos.Add(obj);
                            context.SaveChanges();

                            Guid StuId = obj.StuId;

                            StuStdAcdYearMap ssmap = new StuStdAcdYearMap();
                            ssmap.MapId = Guid.NewGuid();
                            ssmap.StuId = StuId;
                            ssmap.StnId = dto.StnId;
                            ssmap.AcYearId = dto.AcYearId;
                            ssmap.RecDate = DateTime.Now;
                            ssmap.RecType = 0;//New Join
                            context.StuStdAcdYearMaps.Add(ssmap);
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ErrorRegNos.Add(stuinfo.RegdNo);
                        }
                    }
                }
                Success = true;
            }
            catch (Exception exp)
            { 
                
            }

            if (Success == true)
            {
                return Ok(ErrorRegNos);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
