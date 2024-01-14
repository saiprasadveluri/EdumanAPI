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
    public class ExpenseHeadController : ControllerBase
    {
        EduManDBContext context = null;
        public ExpenseHeadController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetExpenseHeadList(Guid OrgId)
        {
            bool Success = false;
            List<ExpenseHeadDTO> dto = null;
            try
            {
                dto = (from obj in context.ExpenseHeads
                       where obj.OrgId == OrgId
                       select new ExpenseHeadDTO()
                       {
                            EHID= obj.EHID,
                            Name=obj.Name,
                            Note=obj.Note,
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
        public ActionResult AddExpenseHead(ExpenseHeadDTO dto)
        {
            bool Success = false;
            try
            {
                ExpenseHead v = new ExpenseHead();
                v.EHID = Guid.NewGuid();
                v.OrgId = dto.OrgId;
                v.Name = dto.Name;
                v.Note = dto.Note;
                v.Status = 1;//Active
                context.ExpenseHeads.Add(v);
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
        public ActionResult EditExpenseHead(ExpenseHeadDTO dto)
        {
            bool Success = false;
            try
            {
                var v = (from obj in context.ExpenseHeads
                         where obj.EHID == dto.EHID
                         select obj).FirstOrDefault();
                if (v != null)
                {
                    v.Name = dto.Name;
                    v.Note = dto.Note;
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

            List<ExpenseHead> CurVens = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurVens = (context.ExpenseHeads.Where(s => OrIds.Contains(s.EHID.ToString()) == true).Select(s => s)).ToList();
                if (CurVens != null)
                {
                    context.ExpenseHeads.RemoveRange(CurVens);
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