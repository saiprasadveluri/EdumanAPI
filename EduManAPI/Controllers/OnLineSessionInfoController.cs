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
    public class OnLineSessionInfoController : ControllerBase
    {
        EduManDBContext context = null;
        public OnLineSessionInfoController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetSessionInfos(Guid? SesId,Guid? OrgId)
        {
            bool Success = false;
            List<OnLineSessionInfoDTO> SesLst = new List<OnLineSessionInfoDTO>();
            try
            {
                if (SesId.HasValue)
                {
                    SesLst = (from obj in context.OnLineSessionInfos
                              join urlobj in context.OnlineSessionUrls on obj.OSessionUrlId equals urlobj.OSUrlId
                              where obj.OSId == SesId.Value
                              select new OnLineSessionInfoDTO()
                              {
                                  OSId = obj.OSId,
                                  SubMapId = obj.SubMapId,
                                  StartDate = obj.StartDate,
                                  EndDate = obj.EndDate,
                                  StartTime = obj.StartTime.ToString(),
                                  EndTime = obj.EndTime.ToString(),
                                  IsReapated = obj.IsReapated,
                                  RepeatString = obj.RepeatString,
                                  OSessionUrlId = obj.OSessionUrlId,
                                  OSessionUrl=urlobj.SessionUrl
                                 
                              }).ToList();
                    Success = true;
                }
                else
                {
                    SesLst = (from obj in context.OnLineSessionInfos
                              join mapobj in context.StdSubMaps on obj.SubMapId equals mapobj.MapId
                              join StdObj in context.Standards on mapobj.StdId equals StdObj.StdId
                              join urlobj in context.OnlineSessionUrls on obj.OSessionUrlId equals urlobj.OSUrlId
                              where StdObj.OrgId==OrgId
                              select new OnLineSessionInfoDTO()
                              {
                                  OSId = obj.OSId,
                                  SubMapId = obj.SubMapId,
                                  StartDate = obj.StartDate,
                                  EndDate = obj.EndDate,
                                  StartTime = obj.StartTime.ToString(),
                                  EndTime = obj.EndTime.ToString(),
                                  IsReapated = obj.IsReapated,
                                  RepeatString = obj.RepeatString,
                                  OSessionUrlId = obj.OSessionUrlId,
                                  OSessionUrl=urlobj.SessionUrl
                              }).ToList();
                    Success = true;
                }
            }
            catch (Exception ex)
            { 
            
            }
            if (Success)
            {
                return Ok(SesLst);
            }
            else
            {
                return BadRequest("Error");
            }

        }
        [HttpPost]
        public ActionResult AddOnlineSession(OnLineSessionInfoDTO dto)
        {
            //OnLineSessionInfoDTO dto = new OnLineSessionInfoDTO();

            bool Success = false;
            try
            {
                OnLineSessionInfo sesobj = new OnLineSessionInfo();

                sesobj.OSId = Guid.NewGuid();
                sesobj.SubMapId = dto.SubMapId;
                sesobj.StartDate = dto.StartDate;
                sesobj.EndDate = dto.EndDate;
                sesobj.StartTime = TimeSpan.Parse(dto.StartTime);
                sesobj.EndTime = TimeSpan.Parse(dto.EndTime);
                sesobj.IsReapated = dto.IsReapated;
                sesobj.RepeatString = dto.RepeatString;
                sesobj.OSessionUrlId = dto.OSessionUrlId;
                context.OnLineSessionInfos.Add(sesobj);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
            {
            }
            if (Success)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteOnlineSessions(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<OnLineSessionInfo> Curs = null;
            try
            {
                List<string> Ids = lst.ToList();
                Curs = (context.OnLineSessionInfos.Where(s => Ids.Contains(s.OSId.ToString()) == true).Select(s => s)).ToList();
                if (Curs != null)
                {
                    context.OnLineSessionInfos.RemoveRange(Curs);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
