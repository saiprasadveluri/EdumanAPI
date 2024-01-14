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
    public class TeacherController : ControllerBase
    {
        EduManDBContext context = null;
        public TeacherController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpDelete]
        public IActionResult DeleteTeachers(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Teacher> CurTechs = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurTechs = (context.Teachers.Where(s => OrIds.Contains(s.TeacherId.ToString()) == true).Select(s => s)).ToList();
                if (CurTechs != null)
                {
                    context.Teachers.RemoveRange(CurTechs);
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

        [HttpGet]
        public ActionResult GetAllTeachers(Guid OrgId)
        {
            List<AddTeacherDTO> Subs = new List<AddTeacherDTO>();
            bool Success = false;
            try
            {
                Subs = context.Teachers.Where(s => s.OrgId == OrgId).
                    Select(s => new AddTeacherDTO()
                    {
                        TeacherId= s.TeacherId,
                        EmpId=s.EmpId,
                        FName=s.FName,
                        MName =s.MName,
                        LName =s.LName,
                        DOJoining=s.DOJoining,
                        Address=s.Address,
                        MobileNo=s.MobileNo,
                        FatherName=s.FatherName,
                        MotherName =s.MotherName,
                        Gender=s.Gender,
                        BllodGroup=s.BllodGroup,
                        TeacherType=s.TeacherType,
                        Status=s.Status
                    }).ToList();
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success)
            {
                return Ok(Subs);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
