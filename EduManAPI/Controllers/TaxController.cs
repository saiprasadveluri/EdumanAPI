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
    public class TaxController : ControllerBase
    {
        EduManDBContext context = null;
        public TaxController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetTaxList(Guid OrgId)
        {
            bool Success = false;
            List<TaxDTO> dto = null;
            try
            {
                dto = (from obj in context.Taxes
                       where obj.OrgId == OrgId
                       select new TaxDTO()
                       {
                        TID=obj.TID,
                        Title = obj.Title,
                        TaxNumber = obj.TaxNumber,
                        Percentage = obj.Percentage,
                        Description= obj.Description
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
        public ActionResult AddTax(TaxDTO dto)
        {
            bool Success = false;
            try
            {
                Tax v = new Tax();
                v.TID = Guid.NewGuid();
                v.OrgId = dto.OrgId;
                v.Title = dto.Title;
                v.TaxNumber = dto.TaxNumber;
                v.Percentage = dto.Percentage;
                v.Description = dto.Description;

                context.Taxes.Add(v);
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
        public ActionResult EditTax(TaxDTO dto)
        {
            bool Success = false;
            try
            {
                var v = (from obj in context.Taxes
                         where obj.TID == dto.TID
                         select obj).FirstOrDefault();
                if (v != null)
                {
                    v.Title = dto.Title;
                    v.TaxNumber = dto.TaxNumber;
                    v.Percentage = dto.Percentage;
                    v.Description = dto.Description;
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
        public IActionResult DeleteTaxes(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Tax> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.Taxes.Where(s => OrIds.Contains(s.TID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    context.Taxes.RemoveRange(CurVens);
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