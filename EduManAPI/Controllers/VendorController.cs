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
    public class VendorController : ControllerBase
    {
        EduManDBContext context = null;
        public VendorController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetVendorList(Guid OrgId)
        {
            bool Success = false;
            List<VendorListEntryDTO> dto = null;
            try
            {
                dto = (from obj in context.Vendors
                       where obj.OrgId == OrgId
                       select new VendorListEntryDTO()
                       { 
                        VendorId = obj.VendorId,
                        VendorName = obj.VendorName,
                        ContactName = obj.ContactName,
                         ContactNo=obj.ContactNo,
                          Location= obj.Location,
                           Address= obj.Address,
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
        public ActionResult AddVendor(VendorListEntryDTO dto)
        {
            bool Success = false;
            try
            {
                Vendor v = new Vendor();
                v.VendorId = Guid.NewGuid();
                v.OrgId = dto.OrgId;
                v.VendorName = dto.VendorName;
                v.ContactName = dto.ContactName;
                v.ContactNo = dto.ContactNo;
                v.Location = dto.Location;
                v.Address = dto.Address;
                v.Status = 1;//Active
                context.Vendors.Add(v);
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

        [HttpPut]
        public ActionResult EditVendor(VendorListEntryDTO dto)
        {
            bool Success = false;
            try
            {
                var v = (from obj in context.Vendors
                         where obj.VendorId == dto.VendorId
                         select obj).FirstOrDefault();
                if(v!=null)
                {
                    v.VendorName = dto.VendorName;
                    v.ContactName = dto.ContactName;
                    v.ContactNo = dto.ContactNo;
                    v.Location = dto.Location;
                    v.Address = dto.Address;
                    v.Status = dto.Status;
                    
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
        public IActionResult DeleteVendors(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Vendor> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.Vendors.Where(s => OrIds.Contains(s.VendorId.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    context.Vendors.RemoveRange(CurVens);
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
