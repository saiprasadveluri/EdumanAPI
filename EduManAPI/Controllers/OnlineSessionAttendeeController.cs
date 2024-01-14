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
    public class OnlineSessionAttendeeController : ControllerBase
    {
        EduManDBContext context = null;
        public OnlineSessionAttendeeController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetOnlineAttendeeList(Guid OSId,Guid AYId,string StrEvtDate)
        {
            bool Success = false;
            DateTime EvtDate = DateTime.Parse(StrEvtDate);
            List<OnlineSessionAttendeeEntryDTO> res = new List<OnlineSessionAttendeeEntryDTO>();
            try
            {
                var PresentMapIds = (from pobj in context.OnlineSessionAttendees
                                     where pobj.OSId == OSId && pobj.EvtDate == EvtDate
                                     select pobj.MapId).ToList();

                res = (from StuMapObj in context.StuStdAcdYearMaps
                       join StuInfoObj in context.StudentInfos on StuMapObj.StuId equals StuInfoObj.StuId
                       join StdMapObj in context.StdSubMaps on StuMapObj.StnId equals StdMapObj.StdId
                       join OSObj in context.OnLineSessionInfos on StdMapObj.MapId equals OSObj.SubMapId
                       where OSObj.OSId == OSId && StuMapObj.AcYearId==AYId
                       select new OnlineSessionAttendeeEntryDTO() {
                       MapId= StuMapObj.MapId,
                       StudentName= StuInfoObj.FName,
                       IsPresent = PresentMapIds.Contains(StuMapObj.MapId)?1:0
                       }).ToList();
                
           Success = true;
            }
            catch (Exception e)
            { 
                
            }
            if (Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest("Error");
            }
         }

        [HttpPost]
        public ActionResult OnlineAttendeesAction(OnlineSessionAttendeeActionDTO dto)
        {
            bool Success = false;
            try
            {
                if (dto.ActionType == 1)//1: Mark Present 0: Mark Abscent.
                {
                    var IsPresent = context.OnlineSessionAttendees.Where(a => a.MapId == dto.MapId && a.EvtDate == dto.EvtDate && dto.OsId == a.OSId).ToList().FirstOrDefault();
                    if (IsPresent == null)
                    {
                        OnlineSessionAttendee osat = new OnlineSessionAttendee();
                        osat.AtId = Guid.NewGuid();
                        osat.OSId = dto.OsId;
                        osat.EvtDate = dto.EvtDate;
                        osat.MapId = dto.MapId;
                        context.OnlineSessionAttendees.Add(osat);
                        context.SaveChanges();
                        Success = true;
                    }
                }
                else if (dto.ActionType == 0)
                {
                    var IsPresent = context.OnlineSessionAttendees.Where(a => a.MapId == dto.MapId && a.EvtDate == dto.EvtDate && dto.OsId == a.OSId).ToList().FirstOrDefault();
                    if (IsPresent != null)
                    {
                        context.OnlineSessionAttendees.Remove(IsPresent);
                        context.SaveChanges();
                        Success = true;                        
                    }
                }
            }
            catch (Exception e)
            {

            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Sevice Error");
            }
        }
        /*[HttpPost]
        public ActionResult AddOnlineAttendees(OnlineSessionAttendeeAddDTO dto)
        {
            bool Success = false;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var obj in dto.MapIds)
                        {
                            OnlineSessionAttendee osat = new OnlineSessionAttendee();
                            osat.OSId = dto.OsId;
                            osat.EvtDate = dto.EvtDate;
                            osat.MapId = obj;
                            context.OnlineSessionAttendees.Add(osat);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        Success = true;
                    }
                    catch (Exception exp)
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
                return Ok("Success");
            }
            else
            {
                return BadRequest("Sevice Error");
            }
        }*/
     }
}
