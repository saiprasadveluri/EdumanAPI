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
    public class ExpenseReportController : ControllerBase
    {
        EduManDBContext context = null;
        public ExpenseReportController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetReport([FromQuery]string FromDate, [FromQuery] string ToDate, [FromQuery] Guid OrgId)
        {
            List<ExpensereportEntry> dto = new List<ExpensereportEntry>();
            bool Success = false;
            try
            {
                DateTime FD = DateTime.Parse(FromDate);
                DateTime TD = DateTime.Parse(ToDate);

                List<TempExpensereportEntry> temp = new List<TempExpensereportEntry>();
                var BillLst = (from obj in context.Bills
                               join hobj in context.ExpenseHeads on obj.ExpHeadId equals hobj.EHID 
                               where obj.OrgId == OrgId && (obj.BillDate >= FD && obj.BillDate <= TD)
                            
                               select new { obj, hobj.Name }).ToList();
                foreach(var o in BillLst)
                {
                    var BillTotal = (from dobj in context.BillLineItems
                                     join pobj in context.ServiceOrProducts on dobj.ProdId equals pobj.PID
                                     where dobj.BID == o.obj.BID
                                     select new { Total = pobj.Price * dobj.Quantity }).ToList().Sum(s=>s.Total);

                    var rec = new TempExpensereportEntry() {
                      HeadId= o.obj.ExpHeadId,
                      ExpHeadName = o.Name,
                      TotalAmount=BillTotal,
                      DiscountType= o.obj.DiscountType,
                      DiscountValue= o.obj.DiscountVal
                    };
                    temp.Add(rec);
                }
                //Calculate the total by Head Name
                var res = temp.GroupBy(s => s.ExpHeadName).ToList();
                foreach(var r in res)
                {
                    var hname = r.Key;
                    var sum = r.Sum(s => s.TotalAmount);
                    var totdisc = r.Sum(s => s.DiscountType > 0 ? (s.DiscountType==1?s.DiscountValue:(s.TotalAmount*s.DiscountValue/100)) : 0);
                    dto.Add(new ExpensereportEntry() { 
                    ExpHeadName= hname,
                    TotalAmount= sum,
                    TotalDsicount=totdisc
                    });
                }

                Success = true;
            }           
            catch(Exception e)
            {

            }
            if (Success)
                return Ok(dto);
            else
                return BadRequest("Error");
        }

        
    }
}
