using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    
    [ApiController]
    [EnableCors("Policy1")]
    public class StudentInfoController : ControllerBase
    {
        EduManDBContext context = null;
        public StudentInfoController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [Route("api/[controller]")]
        [HttpDelete]
        public IActionResult DeleteStudents(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<StudentInfo> CurStds = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurStds = (context.StudentInfos.Where(s => OrIds.Contains(s.StuId.ToString()) == true).Select(s => s)).ToList();
                if (CurStds != null)
                {
                    context.StudentInfos.RemoveRange(CurStds);
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

        [Route("api/[controller]")]
        [HttpPost]
        public ActionResult AddStudentsInfo(AddStudentInfoDTO stuinfo)
        {
            bool Success = false;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    var RID = (from robj in context.Roles
                               where robj.RoleVal == (int)RoleEnum.Student
                               select robj.RoleID).FirstOrDefault();

                    var OrgCode = (from oobj in context.Organizations
                                   where oobj.OrgId == stuinfo.OrgId
                                   select oobj.OrgCode).FirstOrDefault();

                    //Add Login Info....
                    UserInfo uinfo = new UserInfo();
                    uinfo.ID = Guid.NewGuid();
                    uinfo.Name = stuinfo.FName;
                    uinfo.MobNo = stuinfo.FatherMobile;
                    uinfo.Emailid = stuinfo.ParentEmail;
                    uinfo.UserID = OrgCode+"_STU" + stuinfo.RegdNo;
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

                    //Add Student Info...
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
                    ssmap.StnId = stuinfo.StnId;
                    ssmap.AcYearId = stuinfo.AcYearId;
                    ssmap.RecDate = DateTime.Now;
                    ssmap.RecType = 0;//New Join
                    context.StuStdAcdYearMaps.Add(ssmap);
                    context.SaveChanges();


                    foreach(var langDet in stuinfo.StuLangs)
                    {
                        StudentLangMap StuLangObj = new StudentLangMap()
                        {
                            MapId = Guid.NewGuid(),
                            StudentStnAcdMapId= ssmap.MapId,
                            LanguageId = langDet.LangId,
                            LangOrdinal = langDet.OrdinalNumber
                        };
                        context.studentLangMaps.Add(StuLangObj);
                    }
                    context.SaveChanges();
                    transaction.Commit();
                    

                    Success = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }             
            }
            if (Success == true)
            {
                return Ok(stuinfo);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [Route("api/[controller]")]
        [HttpPut]
        public ActionResult EditStudentsInfo(AddStudentInfoDTO stuinfo)
        {
            bool Success = false;
               try
                {
                    StudentInfo obj = (from o in context.StudentInfos
                                      where o.StuId==stuinfo.StuId select o).FirstOrDefault();
                    
                    //obj.RegdNo = stuinfo.RegdNo;
                    obj.FName = stuinfo.FName;
                    obj.MName = stuinfo.MName;
                    obj.LName = stuinfo.LName;
                    obj.DOBirth = stuinfo.DOBirth;
                    //obj.DOAdmission = stuinfo.DOAdmission;
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
                    context.SaveChanges();
                    Success = true;
                }
                catch (Exception ex)
                {
                    
                }
            
            if (Success == true)
            {
                return Ok(stuinfo);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [Route("api/[controller]")]
        [HttpGet]
        public ActionResult GetStudentsInfo([FromQuery] GetStudentInfoReqDTO dto)
        {
            bool Success = false;
            List<StudentInfoDTO> StLst = new List<StudentInfoDTO>();
            try
            {
                StLst = (from stmap in context.StuStdAcdYearMaps
                         join stuinfo in context.StudentInfos on stmap.StuId equals stuinfo.StuId
                         join stdobj in context.Standards on stmap.StnId equals stdobj.StdId
                         join AcdObj in context.AcdYears on stmap.AcYearId equals AcdObj.AcdId
                         where AcdObj.AcdId == dto.AyearId && stdobj.StdId == dto.StdId
                         select new StudentInfoDTO()
                         {
                             MapId= stmap.MapId,
                             StuId= stuinfo .StuId,
                             RegdNo = stuinfo.RegdNo,
                             FName= stuinfo.FName,
                             MName = stuinfo.MName,
                             LName = stuinfo.LName,
                             DOBirth= stuinfo.DOBirth,
                             DOAdmission= stuinfo.DOAdmission,
                             ResAddress=stuinfo.ResAddress,
                             FatherName=stuinfo.FatherName,
                             MotherName=stuinfo.MotherName,
                             FatherMobile=stuinfo.FatherMobile,
                             ParentEmail = stuinfo.ParentEmail,
                             Gender=stuinfo.Gender,
                             BloodGroup=stuinfo.BloodGroup,
                             AadharNo=stuinfo.AadharNo,
                             Religion=stuinfo.Religion,
                             Cast= stuinfo.Cast,
                             SchoolAdmNo=stuinfo.SchoolAdmNo,
                             IsActive= stuinfo.IsActive
                         }).ToList();
                Success = true;
            }
            catch (Exception ex)
            { 
            }
            if (Success)
            {
                return Ok(StLst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [Route("api/StudentInfo/{id}")]
        [HttpGet]
        public ActionResult GetStudentIndividualInfo(Guid id)
        {
            bool Success = false;
            StudentInfoDTO StObj = new StudentInfoDTO();
            try
            {
                StObj = (from stuinfo in context.StudentInfos
                         where stuinfo.StuId==id
                         select new StudentInfoDTO()
                         {
                             StuId = stuinfo.StuId,
                             RegdNo = stuinfo.RegdNo,
                             FName = stuinfo.FName,
                             MName = stuinfo.MName,
                             LName = stuinfo.LName,
                             DOBirth = stuinfo.DOBirth,
                             DOAdmission = stuinfo.DOAdmission,
                             ResAddress = stuinfo.ResAddress,
                             FatherName = stuinfo.FatherName,
                             MotherName = stuinfo.MotherName,
                             FatherMobile = stuinfo.FatherMobile,
                             ParentEmail = stuinfo.ParentEmail,
                             Gender = stuinfo.Gender,
                             BloodGroup = stuinfo.BloodGroup,
                             AadharNo = stuinfo.AadharNo,
                             Religion = stuinfo.Religion,
                             Cast = stuinfo.Cast,
                             SchoolAdmNo = stuinfo.SchoolAdmNo,
                             IsActive = stuinfo.IsActive
                         }).FirstOrDefault();
                Success = true;
            }
            catch (Exception ex)
            {
            }
            if (Success)
            {
                return Ok(StObj);
            }
            else
            {
                return BadRequest("Error");
            }
        }

    }
}
