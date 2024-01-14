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
    public class FeeDefaultersDataController : ControllerBase
    {
        EduManDBContext context = null;
        public FeeDefaultersDataController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public ActionResult GetData(Guid OrgId,Guid AcdYearId, Guid StnId, int TermNo)
        {
            List<FeeDefaulterInfoDTO> dto = new List<FeeDefaulterInfoDTO>();

            var MapLst = (from MObj in context.StuStdAcdYearMaps
                          join StuObj in context.StudentInfos on MObj.StuId equals StuObj.StuId
                          join StnObj in context.Standards on MObj.StnId equals StnObj.StdId
                          where MObj.AcYearId == AcdYearId && MObj.StnId == StnId
                          select new { MObj.MapId, StuObj.FName, StuObj.RegdNo, StnObj.StdName,StnObj.StdId }).ToList();

            var FSchoolLst = (from obj in context.FeeMasters
                             join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                             where hobj.OrgId == OrgId && hobj.FeeType == 1 && obj.TermNo<= TermNo
                              select new { obj,hobj,obj.Amount }).ToList();

            for (int n = 0; n < FSchoolLst.Count(); ++n)
            {
                for (int m = 0; m < MapLst.Count(); ++m)
                {
                    var cons = (from obj in context.FeeConcessions
                                where obj.FeeId == FSchoolLst[n].obj.FeeId && obj.MapId == MapLst[m].MapId
                                select obj.Amount).FirstOrDefault();

                    var AmountPaid = (from obj in context.Chalans
                              join lobj in context.ChalanLineInfos on obj.ChlId equals lobj.ChlId
                              where obj.MapId == MapLst[m].MapId && lobj.FeeId == FSchoolLst[n].obj.FeeId
                              select lobj.PaidAmt).Sum();

                    var due = FSchoolLst[n].Amount-(cons+AmountPaid);
                    var itm = new FeeDefaulterInfoDTO()
                    {
                        Amount = due,
                        StuName = MapLst[m].FName,
                        RegdNo = MapLst[m].RegdNo,
                        Stn = MapLst[m].StdName,
                        HeadName = FSchoolLst[n].hobj.FeeHeadName,
                        Term = FSchoolLst[n].obj.TermNo,
                        FeeAmt = FSchoolLst[n].Amount,
                        Concession= cons
                    };
                    dto.Add(itm);
                }
            }

            /*var ClassLst = (from obj in context.FeeMasters
                            join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                            where hobj.OrgId == OrgId && hobj.FeeType == 2 && obj.StnId==StnId
                            select new { obj, hobj, obj.Amount }).ToList();
            */

            //for (int n = 0; n < ClassLst.Count(); ++n)
            for (int m = 0; m < MapLst.Count(); ++m)
            {
                var ClassLst = (from obj in context.FeeMasters
                                join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                                where hobj.OrgId == OrgId && hobj.FeeType == 2 && obj.StnId ==MapLst[m].StdId && obj.TermNo <= TermNo
                                select new { obj, hobj, obj.Amount }).ToList();

                for (int n = 0; n < ClassLst.Count(); ++n)
                {
                    var cons = (from obj in context.FeeConcessions
                                where obj.FeeId == ClassLst[n].obj.FeeId && obj.MapId == MapLst[m].MapId
                                select obj.Amount).FirstOrDefault();

                    var AmountPaid = (from obj in context.Chalans
                                      join lobj in context.ChalanLineInfos on obj.ChlId equals lobj.ChlId
                                      where obj.MapId == MapLst[m].MapId && lobj.FeeId == ClassLst[n].obj.FeeId
                                      select lobj.PaidAmt).Sum();

                    var due = ClassLst[n].Amount - (cons + AmountPaid);
                    var itm = new FeeDefaulterInfoDTO()
                    {
                        Amount = due,
                        StuName = MapLst[m].FName,
                        RegdNo = MapLst[m].RegdNo,
                        Stn = MapLst[m].StdName,
                        HeadName = ClassLst[n].hobj.FeeHeadName,
                        Term = ClassLst[n].obj.TermNo,
                        FeeAmt = ClassLst[n].Amount,
                        Concession = cons
                    };
                    dto.Add(itm);
                }
            }

            /*var StudentLevelLst = (from obj in context.FeeMasters
                            join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                            where hobj.OrgId == OrgId && hobj.FeeType == 3 
                                   select new { obj, hobj, obj.Amount }).ToList();

            */
            //for (int n = 0; n < StudentLevelLst.Count(); ++n)
            for (int m = 0; m < MapLst.Count(); ++m)
            {
                var StudentLevelLst = (from obj in context.FeeMasters
                                       join hobj in context.FeeHeadMasters on obj.FHeadId equals hobj.FeeHeadId
                                       where hobj.OrgId == OrgId && hobj.FeeType == 3 && obj.MapId==MapLst[m].MapId && obj.TermNo <= TermNo
                                       select new { obj, hobj, obj.Amount }).ToList();
                for (int n = 0; n < StudentLevelLst.Count(); ++n)
                {
                    var cons = (from obj in context.FeeConcessions
                                where obj.FeeId == StudentLevelLst[n].obj.FeeId && obj.MapId == MapLst[m].MapId
                                select obj.Amount).FirstOrDefault();

                    var AmountPaid = (from obj in context.Chalans
                                      join lobj in context.ChalanLineInfos on obj.ChlId equals lobj.ChlId
                                      where obj.MapId == MapLst[m].MapId && lobj.FeeId == StudentLevelLst[n].obj.FeeId
                                      select lobj.PaidAmt).Sum();

                    var due = StudentLevelLst[n].Amount - (cons + AmountPaid);
                    var itm = new FeeDefaulterInfoDTO()
                    {
                        Amount = due,
                        StuName = MapLst[m].FName,
                        RegdNo = MapLst[m].RegdNo,
                        Stn = MapLst[m].StdName,
                        HeadName = StudentLevelLst[n].hobj.FeeHeadName,
                        Term = StudentLevelLst[n].obj.TermNo,
                        FeeAmt = StudentLevelLst[n].Amount,
                        Concession = cons
                    };
                    dto.Add(itm);
                }
            }
            List<FeeDefaulterInfoDTO> ResDto = dto.OrderBy(s => s.RegdNo).ThenBy(s => s.HeadName).ThenBy(s => s.Term).ToList();
            return Ok(ResDto);
        }
    }
}
