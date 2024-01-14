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
    public class HomeworkController : ControllerBase
    {
        EduManDBContext context = null;
        public HomeworkController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetHomeWorks(Guid StnId)
        {
            List<HomeWorkGetListEntryDTO> dto = new List<HomeWorkGetListEntryDTO>();
            bool Success = false;
            try
            {
                dto = (from obj in context.HomeWorks
                          join mapobj in context.StdSubMaps on obj.MapId equals mapobj.MapId
                          join stnobj in context.Standards on mapobj.StdId equals stnobj.StdId
                          join subobj in context.Subjects on mapobj.SubId equals subobj.SubId
                            where mapobj.StdId==StnId
                          select new HomeWorkGetListEntryDTO() { 
                          HWId = obj.HWId,
                          HWDate= obj.HWDate.ToShortDateString(),
                          StnName= stnobj.StdName,
                          SubName = subobj.SubjectName,
                          ClassWorkString= obj.ClassWorkString,
                          HomeWorkString = obj.HomeWorkString
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
                return BadRequest("Service Error");

            }
        }

        [HttpPost]
        public ActionResult AddHomework(List<HomeWorkAddDTO> dto)
        {
            bool Success = false;
            try
            {
                if (dto.Count > 0)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var obj in dto)
                            {
                                HomeWork hw = new HomeWork();
                                hw.HWId = Guid.NewGuid();
                                hw.HWDate = obj.HWDate;
                                hw.MapId = obj.MapId;
                                hw.ClassWorkString = obj.ClassWorkString;
                                hw.HomeWorkString = obj.HomeWorkString;
                                context.HomeWorks.Add(hw);
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
                return BadRequest("Service Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteHomeworks(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<HomeWork> CurHws = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurHws = (context.HomeWorks.Where(s => OrIds.Contains(s.HWId.ToString()) == true).Select(s => s)).ToList();
                if (CurHws != null)
                {
                    context.HomeWorks.RemoveRange(CurHws);
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
