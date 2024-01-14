using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class FeeCollectionController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeCollectionController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetFeeCollectionInfoList(Guid MapId)
        {
            bool Success = false;
            List<FeeCollectionInfoDTO> dto = new List<FeeCollectionInfoDTO>();

            try
            {
                var temp = (from obj in context.FeeCollections
                           where obj.MapId == MapId
                           select obj).ToList();
                for (int n = 0; n < temp.Count; ++n)
                {
                    FeeCollectionInfoDTO col = new FeeCollectionInfoDTO();
                    col.ColId = temp[n].FeeColId;
                    col.PayType = temp[n].PayType;
                    col.Notes = temp[n].Notes;
                    col.ColDate = temp[n].ColDate;
                    dto.Add(col);
                }
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
                return BadRequest("Service error");
            }
        }

        [HttpPost]
        public ActionResult AddFeeCollection(FeeChalanCollectionDTO dto)//FeeCollectionDTO dto)
        {
            bool Success = false;
            string StrFCId = "";
            FeeCollectionReceiptDTO outdto = new FeeCollectionReceiptDTO();
            try
            {                
                var chlnInfo = (from cobj in context.Chalans
                                where cobj.ChlId == dto.ChlnId select cobj).FirstOrDefault();
                if (chlnInfo != null)
                {
                    using (var trns = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //Set the Status Of the Chlana as Paid....
                            chlnInfo.ChalanStatus =(int)ChalanStatusEnum.Paid;
                            FeeCollection fc = new FeeCollection();
                            fc.FeeColId = Guid.NewGuid();
                            fc.ChlnId = dto.ChlnId;
                            fc.MapId = chlnInfo.MapId;//dto.MapId;
                            fc.ColDate = dto.ColDate;
                            fc.PayType = dto.PayType;
                            fc.Notes = dto.Notes;
                            context.FeeCollections.Add(fc);
                            context.SaveChanges();
                            Guid FCId = fc.FeeColId;
                            StrFCId = FCId.ToString();

                            var chlnLineInfo = (from clobj in context.ChalanLineInfos
                                                join cobj in context.Chalans on clobj.ChlId equals cobj.ChlId
                                                where cobj.ChlId == dto.ChlnId
                                                select new {clobj,cobj}).ToList();

                            for (int n = 0; n < chlnLineInfo.Count; ++n)
                            {
                                FeeCollectionLineItem LItem = new FeeCollectionLineItem();
                                LItem.LineItemId = Guid.NewGuid();
                                LItem.ColId = FCId;
                                LItem.FeeId = chlnLineInfo[n].clobj.FeeId;
                                LItem.Amount = chlnLineInfo[n].clobj.PaidAmt;
                               
                                context.FeeCollectionLineItems.Add(LItem);
                                context.SaveChanges();
                            }
                            trns.Commit();
                            outdto.SuccessFalg = 1;
                            GetReceiptData(FCId, outdto);
                            
                        }
                        catch (Exception ex)
                        {
                            trns.Rollback();
                        }
                    }
                }
            }
            catch (Exception exp)
            { 
            
            }
            if (outdto.SuccessFalg>0)
            {
                return Ok(outdto);
            }
            else
            {
                return BadRequest("Service Error");
            }
        }

        private FeeCollectionReceiptDTO GetReceiptData(Guid ColId, FeeCollectionReceiptDTO dto)
        {
            try
            {
                var res = (from fobj in context.FeeCollections
                           join mobj in context.StuStdAcdYearMaps on fobj.MapId equals mobj.MapId
                           join sobj in context.StudentInfos on mobj.StuId equals sobj.StuId
                           join chlObj in context.Chalans on fobj.ChlnId equals chlObj.ChlId
                           where fobj.FeeColId == ColId
                           select new { chlObj.ChlnNumber, sobj.RegdNo, sobj.FName, fobj.PayType, Notes = fobj.Notes }).FirstOrDefault();
                if (res != null)
                {
                    dto.ChlnNumber = res.ChlnNumber;
                    dto.Name = res.FName;
                    dto.RegdNo = res.RegdNo;
                    dto.PayType = res.PayType;
                    dto.Notes = res.Notes;
                    var lres = (from lobj in context.FeeCollectionLineItems
                                join fobj in context.FeeCollections on lobj.ColId equals fobj.FeeColId
                                join fmobj in context.FeeMasters on lobj.FeeId equals fmobj.FeeId
                                join fhobj in context.FeeHeadMasters on fmobj.FHeadId equals fhobj.FeeHeadId
                                where fobj.FeeColId == ColId
                                select new { lobj.Amount, fmobj.TermNo, fhobj.FeeHeadName }).ToList();
                    foreach (var obj in lres)
                    {
                        FeeCollectionReceiptLineDTO temp = new FeeCollectionReceiptLineDTO();
                        temp.FeeHeadName = obj.FeeHeadName;
                        temp.TermNo = obj.TermNo;
                        temp.Amt = obj.Amount;
                        dto.lines.Add(temp);
                    }
                    dto.SuccessFalg = 2;
                }
            }
            catch (Exception e)
            { 
            
            }
            return dto;
        }
    }
}
