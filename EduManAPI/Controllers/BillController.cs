using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class BillController : ControllerBase
    {
        EduManDBContext context = null;
        public BillController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetBillsList(Guid OrgId)
        {
            bool Success = false;
            List<BillDTO> dto = null;
            try
            {
                dto = (from obj in context.Bills
                       join vobj in context.Vendors on obj.VendorId equals vobj.VendorId
                       join ehobj in context.ExpenseHeads on obj.ExpHeadId equals ehobj.EHID 
                       where obj.OrgId == OrgId
                       select new BillDTO()
                       {
                           VendorId = obj.VendorId,
                           VendorName = vobj.VendorName,
                           ExpHeadId = obj.ExpHeadId,
                           ExpHeadName= ehobj.Name,
                           BillDate = obj.BillDate,
                           DueDate = obj.DueDate,
                           BillNumber = obj.BillNumber,
                           FileBase64Content = obj.FileGuid,
                           BID= obj.BID,
                           Note=obj.Note,
                           Status = obj.Status,
                           DiscountType= obj.DiscountType,
                           DiscountVal=obj.DiscountVal
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

        [HttpPut]
        public ActionResult EditBill(BillDTO dto)
        {
            bool Success = false;
            try
            {
                var v = (from obj in context.Bills
                         where obj.BID == dto.BID
                         select obj).FirstOrDefault();
                if (v != null)
                {
                    v.BillDate = dto.BillDate;
                    v.DueDate = dto.DueDate;
                    //v.BillNumber = dto.BillNumber;//We can not change the Bill Number
                    v.ExpHeadId = dto.ExpHeadId;
                    v.VendorId = dto.VendorId;
                    v.Note = dto.Note;
                    v.Status = dto.Status;
                    v.DiscountType = dto.DiscountType;
                    v.DiscountVal = dto.DiscountVal;
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

        [HttpPost]
        public ActionResult AddBill(BillDTO dto)
        {
            bool Success = false;
            string FileGuid = null;
            try
            {
                Bill b = new Bill();
                b.BID = Guid.NewGuid();
                b.OrgId = dto.OrgId;
                b.VendorId = dto.VendorId;
                b.ExpHeadId = dto.ExpHeadId;
                b.BillDate = dto.BillDate;
                b.DueDate = dto.DueDate;
                b.BillNumber = dto.BillNumber;
                b.Note = dto.Note;
                b.Status = dto.Status;
                b.DiscountType = dto.DiscountType;
                b.DiscountVal = dto.DiscountVal;
                if (dto.FileBase64Content!=null && dto.FileBase64Content.Length > 0)
                {
                    FileGuid = SaveFileToDisk(dto.FileBase64Content);
                    b.FileGuid = FileGuid;
                    b.FileMIME = dto.FileMIME;
                }
                context.Bills.Add(b);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception e)
            {
                if(FileGuid!=null)
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

            List<Bill> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.Bills.Where(s => OrIds.Contains(s.BID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    foreach(var rec in  CurVens)
                    {
                        if(rec.FileGuid!=null)
                        {
                            DeleteFile(rec.FileGuid);
                        }
                    }
                    context.Bills.RemoveRange(CurVens);
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

        private string SaveFileToDisk(string Base64Content)
        {
            string FileGuid = null;
            try
            {
                FileGuid = Guid.NewGuid().ToString();
                var FilePath = Directory.GetCurrentDirectory() + "/Bills/" + FileGuid;
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
                var FilePath = Directory.GetCurrentDirectory() + "/Bills/" + FileGuid;
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
