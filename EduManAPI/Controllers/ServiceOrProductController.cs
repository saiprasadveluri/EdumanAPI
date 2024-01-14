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
    public class ServiceOrProductController : ControllerBase
    {
        EduManDBContext context = null;
        public ServiceOrProductController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetProductList(Guid OrgId)
        {
            bool Success = false;
            List<ServiceOrProductDTO> dto = null;
            try
            {
                dto = (from obj in context.ServiceOrProducts
                       join EHObj in context.ExpenseHeads on obj.EHId equals EHObj.EHID
                       join TObj in context.Taxes on obj.TID equals TObj.TID
                       where obj.OrgId == OrgId

                       select new ServiceOrProductDTO()
                       {
                           PID=obj.PID,
                            Name= obj.Name,
                            EHId=obj.EHId,
                            EHName= EHObj.Name,
                            Price= obj.Price,
                            Description= obj.Description,
                            TID = obj.TID,
                            TaxName = TObj.Title                           
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
        public ActionResult AddProduct(ServiceOrProductDTO dto)
        {
            bool Success = false;
            try
            {
                ServiceOrProduct v = new ServiceOrProduct();
                v.PID = Guid.NewGuid();
                v.OrgId = dto.OrgId;
                v.Name = dto.Name;
                v.Price = dto.Price;
                v.Description = dto.Description;
                v.EHId = dto.EHId;
                v.TID = dto.TID;                
                context.ServiceOrProducts.Add(v);
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
        public ActionResult EditProduct(ServiceOrProductDTO dto)
        {
            bool Success = false;
            try
            {
                var v = (from obj in context.ServiceOrProducts
                         where obj.PID == dto.PID
                         select obj).FirstOrDefault();
                if (v != null)
                {
                    v.Name = dto.Name;
                    v.Description = dto.Description;
                    v.Price = dto.Price;
                    v.EHId = dto.EHId;
                    v.TID = dto.TID;
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
        public IActionResult DeleteProduct(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<ServiceOrProduct> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.ServiceOrProducts.Where(s => OrIds.Contains(s.PID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    context.ServiceOrProducts.RemoveRange(CurVens);
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
