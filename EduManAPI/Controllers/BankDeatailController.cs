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
    public class BankDeatailController : ControllerBase
    {
        EduManDBContext context = null;
        public BankDeatailController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetBankDetailsList(Guid OrgId)
        {
            bool Success = false;
            List<BankDeatailDTO> dto = null;
            try
            {
                dto = (from obj in context.BankDeatails
                       where obj.OrgId == OrgId
                       select new BankDeatailDTO()
                       {
                           AccountType= obj.AccountType,
                           CashierName= obj.CashierName,
                           BankName=obj.BankName,
                           BankAccountNo= obj.BankAccountNo,
                           WalletName= obj.WalletName,
                           MobileNo= obj.MobileNo,
                           BID= obj.BID,
                           Status = obj.Status
                       }).ToList();
                Success = true;
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
        public ActionResult AddBankDetails(BankDeatailDTO dto)
        {
            bool Success = false;
            try
            {
                BankDeatail BD = new BankDeatail();
                BD.BID = Guid.NewGuid();
                BD.OrgId = dto.OrgId;
                BD.AccountType = dto.AccountType;

                BD.CashierName = dto.CashierName;

                BD.BankName = dto.BankName;
                BD.BankAccountNo = dto.BankAccountNo;

                BD.WalletName = dto.WalletName;
                BD.MobileNo = dto.MobileNo;

                BD.Status = 1;//Active
                context.BankDeatails.Add(BD);
                context.SaveChanges();
                Success = true;
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
        public ActionResult EditBankDetails(BankDeatailDTO dto)
        {
            bool Success = false;
            try
            {
                var BD = (from obj in context.BankDeatails
                         where obj.BID == dto.BID
                         select obj).FirstOrDefault();
                if (BD != null)
                {
                    //BD.OrgId = dto.OrgId;
                    //BD.AccountType = dto.AccountType;
                    BD.CashierName = dto.CashierName;
                    BD.BankName = dto.BankName;
                    BD.BankAccountNo = dto.BankAccountNo;
                    BD.WalletName = dto.WalletName;
                    BD.MobileNo = dto.MobileNo;
                    BD.Status = dto.Status;
                    context.SaveChanges();
                    Success = true;
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

        [HttpDelete]
        public IActionResult DeleteBanks(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<BankDeatail> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.BankDeatails.Where(s => OrIds.Contains(s.BID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    context.BankDeatails.RemoveRange(CurVens);
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