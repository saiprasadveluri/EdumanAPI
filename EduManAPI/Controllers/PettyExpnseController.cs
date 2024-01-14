using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using System.IO;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class PettyExpnseController : ControllerBase
    {
        EduManDBContext context = null;
        public PettyExpnseController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddExpense(PettyExpnseDTO dto)
        {
            bool Success = false;
            string FileGuid = null;
            try
            {
                PettyExpnse b = new PettyExpnse();
                b.PEID = Guid.NewGuid();
                b.Title = dto.Title;
                b.OrgId = dto.OrgId;
                b.EHeadID = dto.EHeadID;
                b.PaymentMode = dto.PaymentMode;
                b.Amount = dto.Amount;
                b.AccountTypeID = dto.AccountTypeID;
                b.ChequeOrRefNo = dto.ChequeOrRefNo;
                b.PaymentDate = dto.PaymentDate;
                b.PaidTo = dto.PaidTo;
                b.Ms = dto.Ms;
                b.Comments = dto.Comments;


                if (dto.FileBase64Content != null && dto.FileBase64Content.Length > 0)
                {
                    FileGuid = SaveFileToDisk(dto.FileBase64Content);
                    b.FileGuid = FileGuid;
                    b.ContentType = dto.FileMIME;
                }
                context.PettyExpnses.Add(b);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception e)
            {
                if (FileGuid != null)
                {
                    DeleteFile(FileGuid);
                }
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
        public IActionResult DeleteBills(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<PettyExpnse> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.PettyExpnses.Where(s => OrIds.Contains(s.PEID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    foreach (var rec in CurVens)
                    {
                        if (rec.FileGuid != null)
                        {
                            DeleteFile(rec.FileGuid);
                        }
                    }
                    context.PettyExpnses.RemoveRange(CurVens);
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
        public ActionResult GetPettyExpList(Guid OrgId)
        {
            bool Success = false;
            List<PettyExpnseDTO> dto = null;
            try
            {
                dto = (from obj in context.PettyExpnses                       
                       join ehobj in context.ExpenseHeads on obj.EHeadID equals ehobj.EHID
                        join aobj in context.BankDeatails on obj.AccountTypeID equals aobj.BID           
                       where obj.OrgId == OrgId
                       select new PettyExpnseDTO()
                       {
                          PEID=obj.PEID,
                          Title = obj.Title,
                          EHeadID = obj.EHeadID,
                          EHeadName= ehobj.Name,
                          PaymentMode= obj.PaymentMode,
                          AccountTypeID= obj.AccountTypeID,
                          AccountTypeName= GetAccountName(aobj.AccountType,aobj.BankName,aobj.BankAccountNo,aobj.WalletName,aobj.MobileNo,aobj.CashierName),
                          Amount= obj.Amount,
                          ChequeOrRefNo = obj.ChequeOrRefNo,
                          PaymentDate = obj.PaymentDate,
                          PaidTo = obj.PaidTo,
                          Ms=obj.Ms,
                          Comments=obj.Comments,
                          FileGuid= obj.FileGuid
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

        private static string GetAccountName(int AccountType, string BankName,string AccountNo, string WalletName, string MobileNo, string  CashierName)
        {
            string ActString = "";
            switch (AccountType)
            {
                case 1:
                    ActString = CashierName+" [ Cash Account ]";
                    break;
                case 2:
                    ActString = BankName + " [ " + AccountNo + " ]"; ;
                    break;
                case 3:
                    ActString = WalletName+ "[ "+MobileNo+ " ]";
                    break;
                default:
                    ActString = "unknown";
                    break;
            }
            return ActString;
        }

        private string SaveFileToDisk(string Base64Content)
        {
            string FileGuid = null;
            try
            {
                FileGuid = Guid.NewGuid().ToString();
                var FilePath = Directory.GetCurrentDirectory() + "/PettyExpenseBills/" + FileGuid;
                byte[] FileBytes = Convert.FromBase64String(Base64Content);
                FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
                fs.Write(FileBytes, 0, FileBytes.Length);//TODO: Check for File Length check
                fs.Close();
            }
            catch (Exception exp)
            {
                FileGuid = null;
            }
            return FileGuid;
        }

        private void DeleteFile(string FileGuid)
        {
            try
            {
                var FilePath = Directory.GetCurrentDirectory() + "/PettyExpenseBills/" + FileGuid;
                if (System.IO.File.Exists(FilePath))
                {

                    System.IO.File.Delete(FilePath);
                }
            }
            catch (Exception e)
            {

            }
        }

    }
}
