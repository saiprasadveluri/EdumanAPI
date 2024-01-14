using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class StudentChallanController : ControllerBase
    {
        EduManDBContext context = null;
        public StudentChallanController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetStudentChallan(Guid MapId, int TermNo)
        {
            ChalanDTO cdto = new ChalanDTO();
            bool Success = false;
            
            try
            {
                var StuData = (from obj in context.StuStdAcdYearMaps
                               join Yobj in context.AcdYears on obj.AcYearId equals Yobj.AcdId
                               join stuobj in context.StudentInfos on obj.StuId equals stuobj.StuId
                               join stnobj in context.Standards on obj.StnId equals stnobj.StdId
                               where obj.MapId==MapId
                               select new { obj, stuobj.FName, stnobj.StdName, stuobj.RegdNo, Yobj.AcdText }).FirstOrDefault();

                //foreach (var sobj in StuData)
                if(StuData!=null)
                {
                    var FeeConsData = (from obj in context.FeeConcessions
                                       where obj.MapId == MapId
                                       select obj).ToList();

                    var sobj = StuData;

                    cdto.MapId = sobj.obj.MapId;
                    cdto.Name = sobj.FName;
                    cdto.Stndardname = sobj.StdName;
                    cdto.RegdNo = sobj.RegdNo;
                    cdto.AcdYear = sobj.AcdText;

                    var col = from fobj in context.FeeCollections
                              join fcli in context.FeeCollectionLineItems on fobj.FeeColId equals fcli.ColId
                              where fobj.MapId == sobj.obj.MapId
                              group fcli by fcli.FeeId into g
                              select new { FID = g.Key, Amount = g.Sum(s => s.Amount) };

                    var SchoolLevel = (from obj in context.FeeMasters
                                       join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                       where h.FeeType == 1 && obj.TermNo <= TermNo                                       
                                       select new ChalanInfoDTO() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount, Concession=GetConcessionAmount(FeeConsData,MapId,obj.FeeId), Paid = col.Where(c => c.FID == obj.FeeId).Select(a => a.Amount).FirstOrDefault(), DueMon = obj.DueMonthNo }).ToList();

                    cdto.info.AddRange(SchoolLevel);

                    var ClassLevel = (from obj in context.FeeMasters
                                      join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                      where h.FeeType == 2 && obj.StnId == StuData.obj.StnId && obj.TermNo <= TermNo
                                      select new ChalanInfoDTO() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount,Concession=GetConcessionAmount(FeeConsData, MapId, obj.FeeId), Paid = col.Where(c => c.FID == obj.FeeId).Select(a => a.Amount).FirstOrDefault(), DueMon = obj.DueMonthNo }).ToList();

                    cdto.info.AddRange(ClassLevel);

                    var StudentLevel = (from obj in context.FeeMasters
                                        join h in context.FeeHeadMasters on obj.FHeadId equals h.FeeHeadId
                                        where h.FeeType == 3 && obj.MapId == sobj.obj.MapId && obj.TermNo <= TermNo
                                        select new ChalanInfoDTO() { TermNo = obj.TermNo, FID = obj.FeeId, HN = h.FeeHeadName, TotAmt = obj.Amount, Concession= GetConcessionAmount(FeeConsData, MapId, obj.FeeId), Paid = col.Where(c => c.FID == obj.FeeId).Select(a => a.Amount).FirstOrDefault(), DueMon = obj.DueMonthNo }).ToList();

                    cdto.info.AddRange(StudentLevel);
                    
                    Success = true;
                }
            
            }
            catch (Exception e)
            {

            }

            if (Success)
            {
                return Ok(cdto);
            }
            else
            {
                return BadRequest("Server Error");
            }
        }

        [HttpPost]
        public ActionResult AddStudentChallan(StudentChalansInfoDTO dto)
        {
            bool Success = false;
            string NewChlnId = String.Empty;
            try
            {
                var StuRec = (from mobj in context.StuStdAcdYearMaps
                              join sobj in context.StudentInfos on mobj.StuId equals sobj.StuId
                              join aobj in context.AcdYears on mobj.AcYearId equals aobj.AcdId
                              where mobj.MapId == dto.StuMapId
                              select new {aobj, mobj, sobj }).FirstOrDefault();
                if (StuRec != null)
                {

                    //Local function
                    void DeactivateActiveChalans()
                    {                        
                        var ExitingChlns = (from cobj in context.Chalans
                                            where StuRec.mobj.MapId== dto.StuMapId && cobj.ChalanStatus == (int)ChalanStatusEnum.Active
                                            select cobj).ToList();
                        foreach (var chlnObj in ExitingChlns)
                        {
                            chlnObj.ChalanStatus = (int)ChalanStatusEnum.Inactive;
                        }
                        context.SaveChanges();
                    }
                    //Endpoint of Local function def

                    DeactivateActiveChalans();
                    string AcdYear = StuRec.aobj.AcdText;
                    string RegdNo = StuRec.sobj.RegdNo;

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            Chalan ch = new Chalan();
                            ch.ChlId = Guid.NewGuid();
                            ch.MapId = dto.StuMapId;
                            ch.ChlDate = DateTime.Now;

                            var ChlnNumInfo = GetChlnNumber(AcdYear, dto.StuMapId, RegdNo);

                            ch.ChlnNumber = ChlnNumInfo.Item1;
                            ch.SNo = ChlnNumInfo.Item2;

                            context.Chalans.Add(ch);

                            context.SaveChanges();
                            Guid NewChld = ch.ChlId;
                            foreach (var lineobj in dto.Items)
                            {
                                var fres = (from fobj in context.FeeMasters
                                            join fhobj in context.FeeHeadMasters on fobj.FHeadId equals fhobj.FeeHeadId
                                            where fobj.FeeId == lineobj.FId select new { fobj, fhobj }).FirstOrDefault();
                                if (fres != null)
                                {
                                    ChalanLineInfo ln = new ChalanLineInfo();
                                    ln.ChlLineId = Guid.NewGuid();
                                    ln.ChlId = NewChld;
                                    ln.FeeId = fres.fobj.FeeId;
                                    ln.FeeHeadName = fres.fhobj.FeeHeadName;
                                    ln.DueMon = fres.fobj.DueMonthNo;
                                    ln.TermNo = fres.fobj.TermNo;
                                    ln.TotAmt = fres.fobj.Amount;
                                    ln.PaidAmt = lineobj.Amt;
                                    context.ChalanLineInfos.Add(ln);
                                    context.SaveChanges();
                                }
                            }
                            transaction.Commit();
                            Success = true;
                            NewChlnId = ch.ChlId.ToString();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            if (Success)
            {

                return new JsonResult(new {NewChlnId=NewChlnId});
            }
            else
            {
                return BadRequest("Service Error");
            }

        }

        private Tuple<string, int> GetChlnNumber(string AcdYear, Guid MapId, string StuRegdNo)
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
