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
    public class StdSubMapController : ControllerBase
    {
        EduManDBContext context = null;
        public StdSubMapController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]        
        public ActionResult GetMap(Guid? StdId)
        {
            bool Success = false;
            List<StdSubMapDTO> SSMap = new List<StdSubMapDTO>();
            try
            {
                SSMap = (from Mapobj in context.StdSubMaps
                         join SubObj in context.Subjects on Mapobj.SubId equals SubObj.SubId
                         where Mapobj.StdId == StdId
                         select new StdSubMapDTO
                         { 
                            MapId= Mapobj.MapId,
                             StdId = Mapobj.StdId.HasValue==true? Mapobj.StdId.Value.ToString():"",
                         SubId=Mapobj.SubId.HasValue ==true? Mapobj.SubId.Value.ToString():"",
                         SubName= SubObj.SubjectName,
                         SubCode= SubObj.SubCode,
                         IsLanguage= SubObj.IsLanguage,
                             LangOrdinal = SubObj.LangOrdinal
                         }).ToList();
                Success = true;
            }
            catch (Exception e)
            { 
            }
            if (Success == true)
            {
                return Ok(SSMap);
            }
            else
            {
                return BadRequest("error");
            }
        }

        [HttpPost]
        public ActionResult CreateMap(List<StdSubMapDTO> dto)
        {
            bool Success = false;
            try
            {
                foreach(var obj in dto)
                {
                    StdSubMap mp = new StdSubMap();
                    mp.MapId = Guid.NewGuid();
                    mp.StdId = Guid.Parse(obj.StdId);
                    mp.SubId = Guid.Parse(obj.SubId);
                    context.StdSubMaps.Add(mp);
                }
                context.SaveChanges();
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
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteMap(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<StdSubMap> CurMaps = null;
            try
            {
                List<string> MapIds = lst.ToList();// Select(s => long.Parse(s)).ToList();
                CurMaps = (context.StdSubMaps.Where(s => MapIds.Contains(s.MapId.ToString()) == true).Select(s => s)).ToList();
                if (CurMaps != null)
                {
                    context.StdSubMaps.RemoveRange(CurMaps);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception exp)
            {

            }
            if (success)
            {
                return new JsonResult("Success");
            }
            else
            {
                return BadRequest("Error");
            }
        }

    }
}
