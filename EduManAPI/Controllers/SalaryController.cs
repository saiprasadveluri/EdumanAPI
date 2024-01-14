using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class SalaryController : ControllerBase
    {
        EduManDBContext context = null;
        public SalaryController(EduManDBContext ctx)
        {
            context = ctx;
        }

        
        [HttpGet]
        public ActionResult GetSalList(int EmpType,Guid OrgId,int MonthNo,string Year)
        {
            List<SalaryListEntryDTO> dto = new List<SalaryListEntryDTO>();
            bool Success = false;
            try
            {
                switch (EmpType)
                {
                    case 1://Teacher
                        {
                            dto = (from obj in context.Salaries
                                       join tobj in context.Teachers on obj.EmpId equals tobj.TeacherId
                                       where tobj.OrgId == OrgId && obj.MonthNo== MonthNo && obj.Year==Year
                                   select new SalaryListEntryDTO() {
                                        SALID= obj.SALID,
                                        EmpId = tobj.TeacherId,
                                        EmpName = tobj.FName,
                                         MonthNo= obj.MonthNo,
                                         Year= obj.Year,
                                         Status = obj.Status
                                       }).ToList();
                            Success = true;
                            break;
                        }

                }
                
            }
            catch (Exception e)
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
        public ActionResult AddSalary(List<SalaryListEntryDTO> lst)
        {
            bool Success = false;
            try
            {
                foreach (var dto in lst)
                {
                    var ExitingPayHeads = (from obj in context.PayRolls
                                           join hobj in context.Payheads on obj.HID equals hobj.PHID
                                           where obj.EmpID == dto.EmpId
                                           select new GetPayrollDTO()
                                           {
                                               HID = hobj.PHID,
                                               PHType = hobj.Type,
                                               PHName = hobj.Name,
                                               OrgAmount = obj.OrgAmount
                                           }).ToList();

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            Salary sal = new Salary();
                            sal.SALID = Guid.NewGuid();
                            sal.EmpId = dto.EmpId;
                            sal.MonthNo = dto.MonthNo;
                            sal.Year = dto.Year;
                            sal.Status = 0;//Pending.
                            context.Salaries.Add(sal);
                            context.SaveChanges();
                            foreach (var ph in ExitingPayHeads)
                            {
                                SalaryDetail saldet = new SalaryDetail();
                                saldet.SALDetID = Guid.NewGuid();
                                saldet.SalID = sal.SALID;
                                saldet.HeadName = ph.PHName;
                                saldet.HeadType = ph.PHType;
                                saldet.Amount = ph.OrgAmount;
                                context.SalaryDetails.Add(saldet);
                                context.SaveChanges();
                            }
                            transaction.Commit();
                            Success = true;
                        }
                        catch (Exception exp)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception e)
            { 
            
            }

            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        public ActionResult ActOnSalary(ActOnSalaryDTO dto)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = dto.chks.Split(',');

            List<Salary> CurAcds = null;
            try
            {
                List<string> OrIds = lst.ToList();
                CurAcds = (context.Salaries.Where(s => OrIds.Contains(s.SALID.ToString()) == true).Select(s => s)).ToList();
                if (CurAcds != null)
                {
                    if (dto.ActionToBeTaken == 0)
                    {
                        context.Salaries.RemoveRange(CurAcds);
                        context.SaveChanges();
                        success = true;
                    }
                    else
                    {
                        foreach (var rec in CurAcds)
                        {
                            rec.Status = 1;
                        }
                        context.SaveChanges();
                        success = true;
                    }


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
