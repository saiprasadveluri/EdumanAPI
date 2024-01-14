using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class FeeConcessionController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeConcessionController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpDelete]
        public IActionResult DeleteStudentFeeConcession(Guid chk)
        {
            bool Success = false;
            try
            {
                var CurRec = context.FeeConcessions.Where(fc => fc.ConId == chk).FirstOrDefault(); ;
                if(CurRec!=null)
                {
                    context.FeeConcessions.Remove(CurRec);
                    context.SaveChanges();
                    Success = true;
                }
                
            }
            catch (Exception ex)
            {

            }
            if(Success)
            {
                return new JsonResult("Success");
            }
            else
            {
                return BadRequest("Error.Record Not found");
            }
        }
        [HttpGet]
        public IActionResult GetStudentFeeConcession(Guid MapId)
        {
            bool Success = false;
            List<FeeConcessionViewDTO> lst = new List<FeeConcessionViewDTO>();
            try
            {
                lst = (from obj in context.FeeConcessions
                           join Fobj in context.FeeMasters on obj.FeeId equals Fobj.FeeId
                           join FHobj in context.FeeHeadMasters on Fobj.FHeadId equals FHobj.FeeHeadId
                           where obj.MapId == MapId
                           select new FeeConcessionViewDTO{ 
                                                            ConId=obj.ConId,
                                                            Amt= obj.Amount, 
                                                            Reason= obj.Reason,
                                                            FeeHeadName= FHobj.FeeHeadName,
                                                            TermNo = Fobj.TermNo,
                                                            ConcessionType=obj.ConcessionType}).OrderBy(itm=>itm.FeeHeadName).ThenBy(itm=>itm.TermNo).ToList();

                Success = true;
            }
            catch (Exception ex)
            { 

            }
            if (Success == true)
            {
                return Ok(lst);
            }
            else
            {
                return BadRequest("Service Error");
            }
        }
        [HttpPost]
        public IActionResult AddStudentFeeConcession([FromBody]List<FeeConcessionDTO> dto)
        {
            bool Success = false;
            try
            {
                using (var trns = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var obj in dto)
                        {
                            FeeConcession fc = new FeeConcession();
                            fc.ConId = Guid.NewGuid();
                            fc.FeeId = obj.FeeId;
                            fc.MapId = obj.MapId;
                            fc.Amount = obj.Amt;
                            fc.Reason = obj.Reason;
                            fc.ConcessionType = obj.ConcessionType;
                            context.FeeConcessions.Add(fc);
                            context.SaveChanges();
                        }
                        trns.Commit();
                        Success = true;
                    }
                    catch (Exception exp)
                    {
                        trns.Rollback();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            if (Success== true)
            {
                return new JsonResult("Success");
            }
            else
            {
                return BadRequest("Service Error");
            }
        }
    }
}
