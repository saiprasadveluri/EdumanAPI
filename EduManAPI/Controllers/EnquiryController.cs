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
    public class EnquiryController : ControllerBase
    {
        EduManDBContext context = null;
        public EnquiryController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public ActionResult GetEnquiries(long? EnqId)
        {
            bool Success = false;
            List<EnquiryDTO> EnqLst = new List<EnquiryDTO>();
            try
            {
                if (EnqId.HasValue)
                {
                    EnqLst = context.Enquiries.Where(e => e.ID == EnqId).Select(e => new EnquiryDTO()
                    {
                        ID= e.ID,
                        SchoolName = e.SchoolName,
                        ContactName = e.ContactName,
                        Phone= e.Phone,
                        Email=e.Email,
                        EnqDate=e.EnqDate,
                        Status = e.Status
                    }).ToList();
                }
                else
                {
                    EnqLst = context.Enquiries.Select(e => new EnquiryDTO()
                    {
                        ID = e.ID,
                        SchoolName = e.SchoolName,
                        ContactName = e.ContactName,
                        Phone = e.Phone,
                        Email = e.Email,
                        EnqDate = e.EnqDate,
                        Status = e.Status
                    }).ToList();
                }                
                Success = true;
            }
            catch (Exception ex)
            {
                
            }
            if (Success == true)
            {
                return Ok(EnqLst);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        public ActionResult CreateEnquiry(EnquiryDTO dto)
        {
            bool Success = false;
            try
            {
                Enquiry enq = new Enquiry();
                enq.SchoolName = dto.SchoolName;
                enq.ContactName = dto.ContactName;
                enq.Phone = dto.Phone;
                enq.Email = dto.Email;
                enq.EnqDate = dto.EnqDate;
                enq.Status = dto.Status;
                context.Enquiries.Add(enq);
                context.SaveChanges();
                Success = true;
                dto.ID = enq.ID;
            }
            catch (Exception exp)
            { 
                
            }
            if (Success == true)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        public ActionResult EditEnquiry(EnquiryDTO dto)
        {
            bool Success = false;
            try
            {
                Enquiry enq = context.Enquiries.Where(e=>e.ID==dto.ID).FirstOrDefault();
                if (enq != null)
                {
                    enq.SchoolName = dto.SchoolName;
                    enq.ContactName = dto.ContactName;
                    enq.Phone = dto.Phone;
                    enq.Email = dto.Email;
                    enq.EnqDate = dto.EnqDate;
                    enq.Status = dto.Status;
                    
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (Success == true)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
