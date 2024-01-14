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
    public class StudentFeeInfoController : ControllerBase
    {
        EduManDBContext context = null;
        public StudentFeeInfoController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetStudentFeeList(Guid MapId,int TermNo)
        {
            StudentFeeInfoDTO dto = new StudentFeeInfoDTO();
            bool Success = false;
            try
            {
                var StuData = (from obj in context.StuStdAcdYearMaps
                               join Yobj in context.AcdYears on obj.AcYearId equals Yobj.AcdId
                               join stuobj in context.StudentInfos on obj.StuId equals stuobj.StuId
                               join stnobj in context.Standards on obj.StnId equals stnobj.StdId
                               where obj.MapId==MapId
                               select new { obj, stuobj.FName, stnobj.StdName, stuobj.RegdNo, Yobj.AcdText }).FirstOrDefault();
                
                if (StuData != null)
                {
                    dto.Name = StuData.FName;
                    dto.RegdNo = StuData.RegdNo;
                    dto.Stndardname = StuData.StdName;
                    
                    var sobj = StuData;
                    var StnId = StuData.obj.StnId;

                    var FeeConsData = (from obj in context.FeeConcessions
                                      where obj.MapId == MapId
                                      select obj).ToList();

                         var SchoolLevel = (from obj in context.FeeMasters
                                       join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                       where h.FeeType == 1 && obj.TermNo <= TermNo
                                       select new StudentFeeInfoLineItem() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount, ConAmout= GetConcessionAmount(FeeConsData,MapId,obj.FeeId) }).ToList();

                    dto.Lines.AddRange(SchoolLevel);

                    var ClassLevel = (from obj in context.FeeMasters
                                      join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                      where h.FeeType == 2 && obj.StnId == StnId && obj.TermNo <= TermNo
                                      select new StudentFeeInfoLineItem() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount, ConAmout = GetConcessionAmount(FeeConsData, MapId, obj.FeeId) }).ToList();

                    dto.Lines.AddRange(ClassLevel);

                    var StudentLevel = (from obj in context.FeeMasters
                                        join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                        where h.FeeType == 3 && obj.MapId == sobj.obj.MapId && obj.TermNo <= TermNo
                                        select new StudentFeeInfoLineItem() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount, ConAmout = GetConcessionAmount(FeeConsData, MapId, obj.FeeId) }).ToList();

                    dto.Lines.AddRange(StudentLevel);

                    Success = true;
                }
            }
            catch (Exception ex)
            { 
            
            }
            if (Success == true)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Service Error");
            }
        }

        private static double GetConcessionAmount(List<FeeConcession> cons, Guid MapId,Guid FeeId)
        {
            double res = (from obj in cons
                          where obj.MapId == MapId && obj.FeeId == FeeId
                          select obj.Amount).FirstOrDefault();
            return res;
        }
    }
}
