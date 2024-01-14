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
    public class ChalanDetailsController : ControllerBase
    {
        EduManDBContext context = null;
        public ChalanDetailsController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetChalanDetails(Guid ChlnId)
        {
            ChalanDTO dto = new ChalanDTO();
            bool Success = false;
            try
            {
                var temp = (from obj in context.Chalans
                           join mobj in context.StuStdAcdYearMaps on obj.MapId equals mobj.MapId
                           join stnobj in context.Standards on mobj.StnId equals stnobj.StdId
                           join stuobj in context.StudentInfos on mobj.StuId equals stuobj.StuId
                           where obj.ChlId == ChlnId
                           select new { 
                           RegnNo=stuobj.RegdNo,
                           Name=stuobj.FName,
                           StnadardName=stnobj.StdName,
                           ChlnNum=obj.ChlnNumber
                           }).FirstOrDefault();

                dto.Name = temp.Name;
                dto.Stndardname = temp.StnadardName;
                dto.RegdNo = temp.RegnNo;
                dto.ChlnNum = temp.ChlnNum;

                var res = (from obj in context.ChalanLineInfos
                           where obj.ChlId == ChlnId
                           select obj).ToList();
                foreach (var itm in res)
                {
                    ChalanInfoDTO linfo = new ChalanInfoDTO()
                    {
                        FID = itm.FeeId,
                        HN = itm.FeeHeadName,
                        TermNo = itm.TermNo,
                        TotAmt = itm.TotAmt,
                        Paid = itm.PaidAmt,
                        Due = itm.TotAmt - itm.PaidAmt,
                        DueMon = itm.DueMon
                    };
                    dto.info.Add(linfo);
                }

                //Order the data...
                dto.info=dto.info.OrderBy(s => s.HN).ThenBy(s => s.TermNo).ToList();

                Success = true;
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
                return BadRequest("Service error");
            }
        }
    }
}
