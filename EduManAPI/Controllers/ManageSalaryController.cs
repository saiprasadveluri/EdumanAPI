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
    public class ManageSalaryController : ControllerBase
    {
        EduManDBContext context = null;
        public ManageSalaryController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetSalaryDetails(Guid SalId)
        {
            bool Success = false;
            EmployeeSalDTO dto = new EmployeeSalDTO();
            dto.SalId = SalId;
            try
            {
                var Sval = (from sobj in context.Salaries
                            where sobj.SALID == SalId
                            select sobj).FirstOrDefault();
                if (Sval != null)
                {
                    dto.MonthNo = Sval.MonthNo;
                    dto.Year = Sval.Year;
                }
                var res = (from obj in context.SalaryDetails
                           where obj.SalID == SalId
                           select obj).ToList();
                foreach (var o in res)
                {
                    EmployeeSalEntryDTO entry = new EmployeeSalEntryDTO()
                    { 
                      SalDetId= o.SALDetID,
                      HeadName=o.HeadName,
                      HeadType= o.HeadType,
                      Amount= o.Amount
                    };
                    dto.lines.Add(entry);
                }
                Success = true;
            }
            catch (Exception exp)
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
        public ActionResult AddSalHeads(EmployeeSalDTO dto)
        {
            bool Success = false;
            try
            { 
              foreach(var line in dto.lines)
                {
                    SalaryDetail sd = new SalaryDetail();
                    sd.SALDetID = Guid.NewGuid();
                    sd.SalID = dto.SalId;
                    sd.HeadName = line.HeadName;
                    sd.HeadType = line.HeadType;
                    sd.Amount = line.Amount;
                    context.SalaryDetails.Add(sd);
                }
                context.SaveChanges();
                Success = true;
            }
            catch(Exception e)
            {

            }
            if(Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public ActionResult DeleteHeads(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<SalaryDetail> CurSubs = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurSubs = (context.SalaryDetails.Where(s => OrIds.Contains(s.SALDetID.ToString()) == true).Select(s => s)).ToList();
                if (CurSubs != null)
                {
                    context.SalaryDetails.RemoveRange(CurSubs);
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
    }
}

