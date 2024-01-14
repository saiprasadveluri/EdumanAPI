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
    public class TimetableSettingsController : ControllerBase
    {
        EduManDBContext context = null;
        public TimetableSettingsController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetSettings(Guid OrgId)
        {
            bool Success = false;
            List<TimeTableSettingDTO> dto = new List<TimeTableSettingDTO>();
            try
            {
               dto = (from obj in context.TimeTableSettings
                          join StnObj in context.Standards on obj.StnId equals StnObj.StdId
                          where StnObj.OrgId == OrgId
                          select new TimeTableSettingDTO { 
                          TTSID=obj.TTSID,
                          StnId=obj.StnId,
                          WorkingDays=obj.WorkingDays,
                          WorkingHours=obj.WorkingHours,
                          StnString=StnObj.StdName
                          }).ToList();

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

        [HttpPost]
        public ActionResult AddSetting(TimeTableSettingDTO dto)
        {
            bool Success = false;
            try
            {
                TimeTableSetting tts = new TimeTableSetting();
                tts.TTSID = Guid.NewGuid();
                tts.StnId = dto.StnId;
                tts.WorkingDays = dto.WorkingDays;
                tts.WorkingHours = dto.WorkingHours;
                context.TimeTableSettings.Add(tts);
                context.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
            { 
            
            }
            if (Success)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteSettings(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<TimeTableSetting> Curs = null;
            try
            {
                List<string> OrIds = lst.ToList();
                Curs = (context.TimeTableSettings.Where(s => OrIds.Contains(s.TTSID.ToString()) == true).Select(s => s)).ToList();
                if (Curs != null)
                {
                    context.TimeTableSettings.RemoveRange(Curs);
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
