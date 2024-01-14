using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class ChelanController : ControllerBase
    {
        EduManDBContext context = null;
        public ChelanController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GenerateClassChalans(Guid StnId,Guid AcdId,int TermNo)
        {
            List<ChalanDTO> ResDto = new List<ChalanDTO>();
            bool Success = false;
            try
            {
                var StuData = (from obj in context.StuStdAcdYearMaps
                               join Yobj in context.AcdYears on obj.AcYearId equals Yobj.AcdId
                               join stuobj in context.StudentInfos on obj.StuId equals stuobj.StuId
                               join stnobj in context.Standards on obj.StnId equals stnobj.StdId
                               where obj.StnId == StnId && obj.AcYearId == AcdId
                               select new { obj, stuobj.FName, stnobj.StdName, stuobj.RegdNo,Yobj.AcdText }).ToList();
                
                //Local function
                void DeactivateActiveChalans()
                {
                    var StuMapList = StuData.Select(s => s.obj.MapId).ToList();
                    var ExitingChlns = (from cobj in context.Chalans
                                        where StuMapList.Contains(cobj.MapId) && cobj.ChalanStatus == (int)ChalanStatusEnum.Active
                                        select cobj).ToList();
                    foreach(var chlnObj in ExitingChlns)
                    {
                        chlnObj.ChalanStatus = (int)ChalanStatusEnum.Inactive;
                    }
                    context.SaveChanges();
                }
                //Endpoint of Local function def
                //Calling local function
                DeactivateActiveChalans();

                //Generate New Challans
                foreach (var sobj in StuData)
                {
                    ChalanDTO cdto = new ChalanDTO();
                    ResDto.Add(cdto);
                    cdto.MapId = sobj.obj.MapId;
                    cdto.Name = sobj.FName;
                    cdto.Stndardname = sobj.StdName;
                    cdto.RegdNo = sobj.RegdNo;
                    cdto.AcdYear = sobj.AcdText;
                    var MapId = sobj.obj.MapId;

                    var FeeConsData = (from obj in context.FeeConcessions
                                       where obj.MapId == MapId
                                       select obj).ToList();

                    var col = from fobj in context.FeeCollections
                              join fcli in context.FeeCollectionLineItems on fobj.FeeColId equals fcli.ColId
                              where fobj.MapId == sobj.obj.MapId
                              group fcli by fcli.FeeId into g
                              select new { FID = g.Key, Amount = g.Sum(s => s.Amount) };

                    var SchoolLevel = (from obj in context.FeeMasters
                                       join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                       where h.FeeType == 1 && obj.TermNo <= TermNo
                                      
                                       select new ChalanInfoDTO() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount- GetConcessionAmount(FeeConsData, MapId, obj.FeeId), Paid = col.Where(c => c.FID == obj.FeeId).Select(a => a.Amount).FirstOrDefault(), DueMon = obj.DueMonthNo }).ToList();

                    cdto.info.AddRange(SchoolLevel);

                    var ClassLevel = (from obj in context.FeeMasters
                                      join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                      where h.FeeType == 2 && obj.StnId == StnId && obj.TermNo <= TermNo
                                     
                                      select new ChalanInfoDTO() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount- GetConcessionAmount(FeeConsData, MapId, obj.FeeId), Paid = col.Where(c => c.FID == obj.FeeId).Select(a => a.Amount).FirstOrDefault(), DueMon = obj.DueMonthNo }).ToList();

                    cdto.info.AddRange(ClassLevel);

                    var StudentLevel = (from obj in context.FeeMasters
                                        join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                        where h.FeeType == 3 && obj.MapId == sobj.obj.MapId && obj.TermNo <= TermNo
                                       
                                        select new ChalanInfoDTO() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount- GetConcessionAmount(FeeConsData, MapId, obj.FeeId), Paid = col.Where(c => c.FID == obj.FeeId).Select(a => a.Amount).FirstOrDefault(), DueMon = obj.DueMonthNo }).ToList();

                    cdto.info.AddRange(StudentLevel);
                }

                

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach(var rec in ResDto)
                        {
                            Chalan ch = new Chalan();
                            ch.ChlId = Guid.NewGuid();
                            ch.MapId = rec.MapId;
                            ch.ChlDate = DateTime.Now;
                            
                            var ChlnNumInfo = GetChlnNumber(rec.AcdYear, rec.MapId, rec.RegdNo);
                            
                            ch.ChlnNumber = ChlnNumInfo.Item1;
                            ch.SNo = ChlnNumInfo.Item2;

                            rec.ChlnNum = ch.ChlnNumber;

                            context.Chalans.Add(ch);
                            context.SaveChanges();
                            Guid NewChld = ch.ChlId;
                            foreach (var lineobj in rec.info)
                            {
                                ChalanLineInfo ln = new ChalanLineInfo();
                                
                                ln.ChlLineId = Guid.NewGuid();                                
                                ln.ChlId = NewChld;
                                ln.FeeId = lineobj.FID;
                                ln.FeeHeadName = lineobj.HN;
                                ln.DueMon = lineobj.DueMon;
                                ln.TermNo = lineobj.TermNo;
                                ln.TotAmt = lineobj.TotAmt;
                                ln.PaidAmt = lineobj.TotAmt-lineobj.Paid;
                                context.ChalanLineInfos.Add(ln);
                                context.SaveChanges();
                            }
                        }                        
                        transaction.Commit();
                        Success = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }

                }
            }
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok(ResDto);
            }
            else
            {
                return BadRequest("Server Error");
            }
        }

        private Tuple<string, int> GetChlnNumber(string AcdYear, Guid MapId,string StuRegdNo)
        {
            int Sno = 0;

            var temp = context.Chalans.Where(c => c.MapId == MapId);
            
            if (temp.Count() > 0)
            {
                Sno = temp.Max(s => s.SNo);
            }
            
            //Increment the Sno by 1
            Sno++;
            string ChlnNum = AcdYear + "-" + StuRegdNo + "-" + Sno.ToString();
            return Tuple.Create(ChlnNum, Sno);
        }

        private static double GetConcessionAmount(List<FeeConcession> cons, Guid MapId, Guid FeeId)
        {
            double res = (from obj in cons
                          where obj.MapId == MapId && obj.FeeId == FeeId
                          select obj.Amount).FirstOrDefault();
            return res;
        }

        
    }
}
