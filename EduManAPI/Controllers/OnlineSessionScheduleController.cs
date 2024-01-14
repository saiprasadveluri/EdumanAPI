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
    public class OnlineSessionScheduleController : ControllerBase
    {
        EduManDBContext context = null;
        public OnlineSessionScheduleController(EduManDBContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public ActionResult GetSchedule([FromQuery] string SD, [FromQuery] string ED,[FromQuery] Guid OrgId)
        {
            bool Success = false;
            List<OnLineSessionInfoDTO> Lst = new List<OnLineSessionInfoDTO>();
            try
            {
                DateTime ReqStartdate = DateTime.Parse(SD);
                DateTime ReqEndDate = DateTime.Parse(ED);

                List<OnLineSessionInfoDTO> OneTimeEvts = (from obj in context.OnLineSessionInfos
                            join mobj in context.StdSubMaps on obj.SubMapId equals mobj.MapId
                            join StnObj in context.Standards on mobj.StdId equals StnObj.StdId
                            join SubObj in context.Subjects on mobj.SubId equals SubObj.SubId
                            join uobj in context.OnlineSessionUrls on obj.OSessionUrlId equals uobj.OSUrlId
                            join tobj in context.Teachers on uobj.TeacherId equals tobj.TeacherId                                  
                            where obj.StartDate>=ReqStartdate && obj.StartDate<=ReqEndDate && StnObj.OrgId==OrgId && obj.IsReapated==0 
                           select new OnLineSessionInfoDTO() { 
                           OSId=obj.OSId,
                           TeachName = tobj.FName,
                           EvtDate= obj.StartDate,
                              
                           StartDate = obj.StartDate,
                           EndDate = obj.EndDate,
                           Standard= StnObj.StdName,
                           StdId = StnObj.StdId.ToString(),

                               Subject= SubObj.SubjectName,
                           StartTime = obj.StartTime.ToString(),
                           EndTime = obj.EndTime.ToString(),
                           SesUrl= uobj.SessionUrl                           
                           }).ToList();

                List<OnLineSessionInfoDTO> RepeatEvts = new List<OnLineSessionInfoDTO>();

                var temprepeated= (from obj in context.OnLineSessionInfos
                             join mobj in context.StdSubMaps on obj.SubMapId equals mobj.MapId
                             join StnObj in context.Standards on mobj.StdId equals StnObj.StdId
                             join SubObj in context.Subjects on mobj.SubId equals SubObj.SubId
                             join uobj in context.OnlineSessionUrls on obj.OSessionUrlId equals uobj.OSUrlId
                             join tobj in context.Teachers on uobj.TeacherId equals tobj.TeacherId
                             where StnObj.OrgId == OrgId && obj.IsReapated==1
                             select new OnLineSessionInfoDTO()
                             {
                                 OSId = obj.OSId,
                                 TeachName = tobj.FName,
                                 EvtDate = obj.StartDate,
                                 StartDate = obj.StartDate,
                                 EndDate = obj.EndDate,
                                 Standard = StnObj.StdName,
                                 StdId = StnObj.StdId.ToString(),
                                 Subject = SubObj.SubjectName,
                                 StartTime = obj.StartTime.ToString(),
                                 EndTime = obj.EndTime.ToString(),
                                 SesUrl = uobj.SessionUrl,
                                 RepeatString = obj.RepeatString
                             }).ToList();

                            foreach (var obj in temprepeated)
                            {
                                string repstring = obj.RepeatString;
                                if (repstring != null && repstring.Length > 0)
                                {
                                    List<int> RepArr = repstring.Split(',').AsEnumerable().Select(s => int.Parse(s)).ToList();

                                    DateTime LimitStartDate;
                                    DateTime LimitEndDate;
                                    
                                    LimitStartDate = obj.StartDate;
                        
                                    if (obj.StartDate < ReqStartdate)
                                        LimitStartDate = ReqStartdate;
                                    else
                                        LimitStartDate = obj.StartDate;

                                    if (obj.EndDate < ReqEndDate)
                                        LimitEndDate = obj.EndDate;
                                    else
                                        LimitEndDate = ReqEndDate;


                                    for (DateTime dt = LimitStartDate; dt <= LimitEndDate; dt = dt.AddDays(1))
                                    {
                                        if (RepArr.Contains((int)dt.DayOfWeek) == true)
                                        {
                                            RepeatEvts.Add(new OnLineSessionInfoDTO() {
                                                OSId = obj.OSId,
                                                TeachName = obj.TeachName,
                                                EvtDate = dt,
                                                StartDate = obj.StartDate,
                                                EndDate = obj.EndDate,
                                                Standard = obj.Standard,
                                                StdId = obj.StdId,
                                                Subject = obj.Subject,
                                                StartTime = obj.StartTime,
                                                EndTime = obj.EndTime,
                                                SesUrl = obj.SesUrl
                                            });
                                        }
                                    }
                                }
                            }
                        if(OneTimeEvts.Count>0)
                            Lst.AddRange(OneTimeEvts);
                        
                        if(RepeatEvts.Count>0)
                            Lst.AddRange(RepeatEvts);
                        
                        if(Lst.Count>0)
                            Lst = Lst.OrderBy(s => s.EvtDate).ThenBy(s=>s.TeachName).ToList();

                Success = true;
            }
            catch (Exception ex)
            { 
                
            }

            if (Success)
            {
                return Ok(Lst);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
