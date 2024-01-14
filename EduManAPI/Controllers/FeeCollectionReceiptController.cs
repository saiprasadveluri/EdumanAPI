using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    //[Route("api/[controller]/{id?}")]
    
    [ApiController]
    [EnableCors("Policy1")]
    public class FeeCollectionReceiptController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeCollectionReceiptController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [Route("api/FeeCollectionReceipt/{id}")]
        [HttpGet]        
        public IActionResult GetFeeReceiptDetails(Guid id)
        {
            bool Success = false;
            FeeCollectionReceiptDTO dto = new FeeCollectionReceiptDTO();
                try
                {
                    var res = (from fobj in context.FeeCollections
                               join mobj in context.StuStdAcdYearMaps on fobj.MapId equals mobj.MapId
                               join sobj in context.StudentInfos on mobj.StuId equals sobj.StuId
                               where fobj.FeeColId == id
                               select new { sobj.RegdNo, sobj.FName, fobj.PayType, Notes = fobj.Notes }).FirstOrDefault();
                    if (res != null)
                    {
                        dto.Name = res.FName;
                        dto.RegdNo = res.RegdNo;
                        dto.PayType = res.PayType;
                        dto.Notes = res.Notes;
                        var lres = (from lobj in context.FeeCollectionLineItems
                                    join fobj in context.FeeCollections on lobj.ColId equals fobj.FeeColId
                                    join fmobj in context.FeeMasters on lobj.FeeId equals fmobj.FeeId
                                    join fhobj in context.FeeHeadMasters on fmobj.FHeadId equals fhobj.FeeHeadId
                                    where fobj.FeeColId == id
                                    select new { lobj.Amount, fmobj.TermNo, fhobj.FeeHeadName }).ToList();
                        foreach (var obj in lres)
                        {
                            FeeCollectionReceiptLineDTO temp = new FeeCollectionReceiptLineDTO();
                            temp.FeeHeadName = obj.FeeHeadName;
                            temp.TermNo = obj.TermNo;
                            temp.Amt = obj.Amount;
                            dto.lines.Add(temp);
                        }
                    Success = true;
                    }
                }
                catch (Exception e)
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

        [Route("api/FeeCollectionReceipt")]
        [HttpGet]
        public IActionResult GetFeeReceipts(Guid AcdYear, Guid StnId, DateTime StartDate,DateTime EndDate,int id=0)
        {
            List<FeeReceiptInfoDTO> dto = new List<FeeReceiptInfoDTO>();
            bool Success = false;
            try
            {
                var res = (from obj in context.FeeCollections
                          join mapobj in context.StuStdAcdYearMaps on obj.MapId equals mapobj.MapId
                          join stnObj in context.Standards on mapobj.StnId equals stnObj.StdId
                          join Stuobj in context.StudentInfos on mapobj.StuId equals Stuobj.StuId
                          join chlobj in context.Chalans on obj.ChlnId equals chlobj.ChlId
                          where mapobj.AcYearId==AcdYear && mapobj.StnId==StnId && (obj.ColDate>=StartDate && obj.ColDate<=EndDate)
                          select new
                          {
                              FeeColId = obj.FeeColId,
                              RegdNo = Stuobj.RegdNo,
                              Name = Stuobj.FName,
                              ColDate = obj.ColDate,
                              Standard = stnObj.StdName,
                              PayType = obj.PayType,
                              ChlNo=chlobj.ChlnNumber
                          }).ToList();

                
                foreach (var rec in res)
                {
                    FeeReceiptInfoDTO temp = new FeeReceiptInfoDTO() { 
                    FeeColId= rec.FeeColId,
                    StuRegdNo = rec.RegdNo,
                    StuName=rec.Name,
                    Standard=rec.Standard,
                    PayType=rec.PayType,
                    ColDate=rec.ColDate,
                    ChlnNumber= rec.ChlNo
                    };
                    double amt = (from lobj in context.FeeCollectionLineItems
                               join obj in context.FeeCollections on lobj.ColId equals obj.FeeColId
                               where obj.FeeColId == temp.FeeColId
                               select lobj).Sum(s => s.Amount);
                    temp.Amount = amt;

                     dto.Add(temp);
                }
                Success = true;
            }
            catch (Exception exp)
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
    }
}
