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
    public class BillLineItemController : ControllerBase
    {
        EduManDBContext context = null;
        public BillLineItemController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetBillLineItemList(Guid BID)
        {
            bool Success = false;
            List<BillLineItemDTO> dto = null;
            try
            {
                dto = (from obj in context.BillLineItems
                       join iobj in context.ServiceOrProducts on obj.ProdId equals iobj.PID
                       join tobj in context.Taxes on obj.TaxId equals tobj.TID
                       where obj.BID == BID
                       select new BillLineItemDTO()
                       {
                           BLID= obj.BLID,
                           BID=obj.BID,
                           ProdId=obj.ProdId,
                           ProdName= iobj.Name,
                           TaxId= obj.TaxId,
                           TaxName = String.Format("{0}-{1}",tobj.Title,tobj.TaxNumber),
                           Quantity=obj.Quantity,
                           Description=obj.Description
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
        public ActionResult EditBillLineItem(BillLineItemDTO dto)
        {
            bool Success = false;
            try
            {
                var v = (from obj in context.BillLineItems
                         where obj.BLID == dto.BLID
                         select obj).FirstOrDefault();
                if (v != null)
                {
                    v.ProdId = dto.ProdId;
                    v.TaxId = dto.TaxId;
                    v.Quantity = dto.Quantity;
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

        [HttpPost]
        public ActionResult AddBillLineItem(BillLineItemDTO dto)
        {
            bool Success = false;
            try
            {
                BillLineItem b = new BillLineItem();
                b.BLID = Guid.NewGuid();
                b.BID = dto.BID;
                b.ProdId = dto.ProdId;
                b.TaxId = dto.TaxId;
                b.Description = dto.Description;
                b.Quantity = dto.Quantity;
                
                context.BillLineItems.Add(b);
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

        [HttpDelete]
        public IActionResult DeleteBillLineItems(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<BillLineItem> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.BillLineItems.Where(s => OrIds.Contains(s.BLID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    context.BillLineItems.RemoveRange(CurVens);
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
