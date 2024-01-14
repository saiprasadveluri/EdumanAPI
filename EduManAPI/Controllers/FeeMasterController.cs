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
    public class FeeMasterController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeMasterController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddTermFees(FeeMasterDTO dto)
        {
            bool Success = false;
            try
            {
                int ExistingRecCount = -1;
                switch (dto.AddMode)
                { 
                    case 1://School Level Fee Head
                        ExistingRecCount = context.FeeMasters.Where(r => (r.FHeadId == dto.FHeadId && r.TermNo == dto.TermNo && r.AcdyearId == dto.AcdyearId)).Count();
                    break;
                    case 2://Class Level Fee Head
                        ExistingRecCount = context.FeeMasters.Where(r => (r.FHeadId == dto.FHeadId && r.TermNo == dto.TermNo && r.AcdyearId == dto.AcdyearId && r.StnId==dto.StnId)).Count();
                        break;
                    case 3://Class Level Fee Head
                        ExistingRecCount = context.FeeMasters.Where(r => (r.FHeadId == dto.FHeadId && r.TermNo == dto.TermNo && r.AcdyearId == dto.AcdyearId && r.MapId == dto.MapId)).Count();
                        break;
                }

                if (ExistingRecCount == 0)
                {
                    FeeMaster fm = new FeeMaster();
                    fm.FeeId = Guid.NewGuid();
                    fm.FHeadId = dto.FHeadId;
                    fm.TermNo = dto.TermNo;
                    fm.StnId = dto.StnId;
                    fm.MapId = dto.MapId;
                    fm.Amount = dto.Amount;
                    fm.AcdyearId = dto.AcdyearId;
                    fm.DueDayNo = 5;// dto.DueDayNo; Hard-Coded value
                    fm.DueMonthNo = dto.DueMonthNo;
                    context.FeeMasters.Add(fm);
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception exp)
            {
            }

            if (Success)
                return new JsonResult(dto);
            else
                return BadRequest("Service Error");
        }

        /*[HttpGet]
        public ActionResult GetTermFeeList(Guid Id,int GetType)
        {
            List<FeeMasterDTO> dto = new List<FeeMasterDTO>();
            bool Success = false;
            try
            {
                if (GetType==1)//Fetch All Terms fro the given HeadId
                {
                    dto = (from hobj in context.FeeMasters
                           join AcdObj in context.AcdYears on hobj.AcdyearId equals AcdObj.AcdId
                           join StnObj in context.Standards on hobj.StnId equals StnObj.StdId
                           where hobj.FHeadId== Id
                           select new FeeMasterDTO()
                           {
                              FeeId= hobj.FeeId,
                              FHeadId=hobj.FHeadId,
                              TermNo = hobj.TermNo,
                              StnId = hobj.StnId,
                               StnText=StnObj.StdName,
                              MapId =hobj.MapId,
                              Amount = hobj.Amount,
                               AcdyearId= hobj.AcdyearId,
                               AcdYearText= AcdObj.AcdText,
                               DueMonthNo =hobj.DueMonthNo,
                               DueDayNo=hobj.DueDayNo
                           }).ToList();
                }
                else//fetch specific FeeTerm
                {
                    dto = (from hobj in context.FeeMasters
                           join AcdObj in context.AcdYears on hobj.AcdyearId equals AcdObj.AcdId
                           join StnObj in context.Standards on hobj.StnId equals StnObj.StdId
                           where hobj.FeeId==Id
                           select new FeeMasterDTO()
                           {
                               FeeId = hobj.FeeId,
                               FHeadId = hobj.FHeadId,
                               TermNo = hobj.TermNo,
                               StnId = hobj.StnId,
                               StnText = StnObj.StdName,
                               MapId = hobj.MapId,
                               Amount = hobj.Amount,
                               AcdyearId = hobj.AcdyearId,
                               AcdYearText = AcdObj.AcdText,
                               DueMonthNo = hobj.DueMonthNo,
                               DueDayNo = hobj.DueDayNo
                           }).ToList();
                }
                Success = true;
            }
            catch (Exception exp)
            {

            }
            if (Success == true)
                return Ok(dto);
            else
                return BadRequest("Service error");
        }*/
        [HttpDelete]
        public ActionResult DeleteFeeRecord(Guid id)
        {
            bool Success = false;
            try
            {
                var CurRec = context.FeeMasters.Where(r => r.FeeId == id).FirstOrDefault();
                if(CurRec != null)
                {
                    context.FeeMasters.Remove(CurRec);
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch(Exception exp)
            {

            }
            if (Success)
            {
                return new JsonResult("Success");
            }
            else
                return BadRequest("Error");
        }
        [HttpGet]
        public ActionResult GetTermFeeList(Guid Id,int GetType)
        {
            List<FeeMasterDTO> dto = new List<FeeMasterDTO>();
            bool Success = false;
            try
            {
                if (GetType==1)//Fetch All Terms fro the given HeadId
                {
                    dto = (from hobj in context.FeeMasters
                           join AcdObj in context.AcdYears on hobj.AcdyearId equals AcdObj.AcdId
                           from StnObj in context.Standards.Where(p => hobj.StnId == p.StdId).DefaultIfEmpty()
                           from MapObj in context.StuStdAcdYearMaps.Where(p => hobj.MapId == p.MapId).DefaultIfEmpty()
                           from StuObj in context.StudentInfos.Where(p=>MapObj.StuId == p.StuId).DefaultIfEmpty()
                           from StnObj2 in context.Standards.Where(p => MapObj.StnId == p.StdId).DefaultIfEmpty()
                           where hobj.FHeadId== Id
                           select new FeeMasterDTO()
                           {
                              FeeId= hobj.FeeId,
                              FHeadId=hobj.FHeadId,
                              TermNo = hobj.TermNo,
                              StnId = hobj.StnId,
                               StnText=StnObj.StdName?? StnObj2.StdName,
                              MapId =hobj.MapId,
                               StudentDispName = StuObj.FName,
                               Amount = hobj.Amount,
                               AcdyearId= hobj.AcdyearId,
                               AcdYearText= AcdObj.AcdText,
                               DueMonthNo =hobj.DueMonthNo,
                               DueDayNo=hobj.DueDayNo
                           }).ToList();
                }
                else//fetch specific FeeTerm
                {
                    dto = (from hobj in context.FeeMasters
                           join AcdObj in context.AcdYears on hobj.AcdyearId equals AcdObj.AcdId
                           from StnObj in context.Standards.Where(p => hobj.StnId == p.StdId).DefaultIfEmpty()
                           from MapObj in context.StuStdAcdYearMaps.Where(p => hobj.MapId == p.MapId).DefaultIfEmpty()
                           from StuObj in context.StudentInfos.Where(p => MapObj.StuId == p.StuId).DefaultIfEmpty()
                           from StnObj2 in context.Standards.Where(p => MapObj.StnId == p.StdId).DefaultIfEmpty()
                           where hobj.FeeId==Id
                           select new FeeMasterDTO()
                           {
                               FeeId = hobj.FeeId,
                               FHeadId = hobj.FHeadId,
                               TermNo = hobj.TermNo,
                               StnId = hobj.StnId,
                               StnText = StnObj.StdName ?? StnObj2.StdName,
                               StudentDispName =StuObj.FName,
                               MapId = hobj.MapId,
                               Amount = hobj.Amount,
                               AcdyearId = hobj.AcdyearId,
                               AcdYearText = AcdObj.AcdText,
                               DueMonthNo = hobj.DueMonthNo,
                               DueDayNo = hobj.DueDayNo
                           }).ToList();
                }
                Success = true;
            }
            catch (Exception exp)
            {

            }
            if (Success == true)
                return Ok(dto);
            else
                return BadRequest("Service error");
        }

    }
}
