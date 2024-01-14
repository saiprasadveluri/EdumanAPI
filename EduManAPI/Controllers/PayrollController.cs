using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class PayrollController : ControllerBase
    {
        EduManDBContext context = null;
        public PayrollController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [Route("api/Payroll/{EmpType}")]
        [HttpGet]
        public ActionResult GetPayRolls(int EmpType)//1: Teacher 2: Cashier etc...
        {
            bool Success = false;
            List<EmployeeWithPayrollListDTO> lst = new List<EmployeeWithPayrollListDTO>();
            try
            {
                switch (EmpType)
                {
                    case 1:
                        {
                            lst = (from obj in context.PayRolls
                                   join tobj in context.Teachers on obj.EmpID equals tobj.TeacherId
                                   select new EmployeeWithPayrollListDTO()
                                   { 
                                    EmpGUID= tobj.TeacherId,
                                    EmpName = tobj.FName
                                   }).ToList();
                            Success = true;
                        }
                        break;
                }
            }
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok(lst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [Route("api/[controller]")]
        [HttpGet]
        public IActionResult GetEmpPayroll(Guid EmpId)
        {
            try
            {
                var ExitingPayHeads = (from obj in context.PayRolls
                                   join hobj in context.Payheads on obj.HID equals hobj.PHID
                                   where obj.EmpID == EmpId
                                   select new GetPayrollDTO()
                                   {
                                       HID=hobj.PHID,
                                       PHType = hobj.Type,
                                       PHName=hobj.Name,
                                       OrgAmount= obj.OrgAmount
                                   }).ToList();
                var PHlst = (from obj in ExitingPayHeads
                             select obj.HID).ToList();

                var NonExtingPayheads = (from obj in context.Payheads
                                         where PHlst.Contains(obj.PHID) == false
                                         select new GetPayrollDTO()
                                         {
                                             HID = obj.PHID,
                                             PHType = obj.Type,
                                             PHName = obj.Name,
                                             OrgAmount = 0
                                         }).ToList();


                var res = ExitingPayHeads.Union(NonExtingPayheads).ToList(); ;

                return Ok(res);
            }
            catch (Exception e)
            { 
                
            }
            return BadRequest("Error");
        }

        [Route("api/[controller]")]
        [HttpPost]
        public ActionResult AddPayroll(List<PayrollDTO> model)
        {
            bool Success = false;

            try
            {
                var empid = model.Select(p => p.EmpID).FirstOrDefault();
                if (empid != null)
                {
                    var ExistingPHIds = context.PayRolls.Where(p => p.EmpID == empid).Select(p => p.HID).ToList();
                    var SelectedIds = model.Select(p => p.HID).ToList();
                    var ToUpdateHids = ExistingPHIds.Intersect(SelectedIds).ToList();
                    var ToAddHids = SelectedIds.Where(s => ExistingPHIds.Contains(s) == false).ToList();
                    
                    var ToAddEntries = model.Where(s => ToAddHids.Contains(s.HID)).Select(s => s).ToList();
                    //var ToUpdateEntries = context.PayRolls.Where(p => ToAddHids.Contains(p.HID) && p.EmpID == empid).Select(p => p.HID).ToList();
                    //Add the new entries
                    foreach (var itm in ToAddEntries)
                    {
                        PayRoll pr = new PayRoll();
                        pr.PRID = Guid.NewGuid();
                        pr.EmpID = itm.EmpID;
                        pr.HID = itm.HID;
                        pr.OrgAmount = itm.OrgAmount;
                        pr.Status = 1;
                        context.PayRolls.Add(pr);
                    }
                    //To Update Entries
                    foreach (var itm in ToUpdateHids)
                    {
                        PayRoll pr = context.PayRolls.Where(s => s.HID == itm && s.EmpID==empid).FirstOrDefault();
                        if (pr != null)
                        {
                            var newAmt = model.Where(s => s.HID == itm).Select(s => s.OrgAmount).FirstOrDefault();
                            pr.OrgAmount = newAmt;
                        }
                        //pr.Status = 1;
                        //context.PayRolls.Add(pr);
                    }
                    context.SaveChanges();
                    Success = true;
                }
                }
                catch (Exception e)
                { 
            
                }
                if (Success)
                    return Ok("Success");
                else
                    return BadRequest("Error");
            }

    }
}
