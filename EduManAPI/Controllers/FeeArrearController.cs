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
    public class FeeArrearController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeArrearController(EduManDBContext ctx)
        {
            context = ctx;
        }


        [HttpGet]
        public ActionResult GetFeeArrears(Guid MapId)
        {
            bool success = false;
            List<FeeArrearDTO> dto = new List<FeeArrearDTO>();
            try
            {
                var StuData = (from obj in context.StuStdAcdYearMaps
                              where obj.MapId == MapId
                               select obj).FirstOrDefault();
                Guid StuId = StuData.StuId;
                Guid StnId = StuData.StnId;
                Guid AcdId = StuData.AcYearId;

                var SchoolLevel = (from obj in context.FeeMasters
                                 join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                                 where obj.AcdyearId == AcdId && hobj.FeeType==1
                                 select new FeeDataHolder() { 
                                 FeeId = obj.FeeId,
                                 HeadName= hobj.FeeHeadName,
                                 Amount = obj.Amount,
                                 TermNo = obj.TermNo
                                 }).ToList();

                var ClassLevel = (from obj in context.FeeMasters
                                  join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                                  where obj.AcdyearId == AcdId && hobj.FeeType == 2 && obj.StnId==StnId
                                  select new FeeDataHolder()
                                  {
                                      FeeId = obj.FeeId,
                                      HeadName = hobj.FeeHeadName,
                                      Amount = obj.Amount,
                                      TermNo = obj.TermNo
                                  }).ToList();

                var StudentLevel = (from obj in context.FeeMasters
                                  join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                                  where obj.AcdyearId == AcdId && hobj.FeeType == 3 && obj.MapId == MapId
                                  select new FeeDataHolder()
                                  {
                                      FeeId = obj.FeeId,
                                      HeadName = hobj.FeeHeadName,
                                      Amount = obj.Amount,
                                      TermNo = obj.TermNo
                                  }).ToList();

                var Feedata=StudentLevel.Union(ClassLevel).Union(SchoolLevel);

                
                var FeePaidData = (from obj in context.FeeCollections
                                   join LIObj in context.FeeCollectionLineItems on obj.FeeColId equals LIObj.ColId
                                   where obj.MapId == MapId
                                   select LIObj.FeeId).ToList();

                //Get the records in Feedata and not in FeePaidData sequence
                var res = Feedata.Where(p => FeePaidData.Contains(p.FeeId) == false);

                dto = (from obj in res
                                          select new FeeArrearDTO() { 
                                            FeeId=obj.FeeId,
                                            HeadName= obj.HeadName,
                                            TermNo= obj.TermNo,
                                            Amount = obj.Amount
                                          }).ToList();
                success = true;
            }
            catch (Exception exp)
            { 
                
            }
            if (success)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Service Error");

            }
        }
    }

    public class FeeDataHolder
    {
        public Guid FeeId;
        public string HeadName;
        public double Amount;
        public int TermNo;
    }

}
