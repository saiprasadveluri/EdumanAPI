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
    public class SubChapeterController : ControllerBase
    {
        EduManDBContext context = null;
        public SubChapeterController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddChapter(AddSubChapeterDTO dto)
        {
            bool Success = false;
            try
            {   
                SubChapeter chap = new SubChapeter();
                chap.ChapId = Guid.NewGuid();
                chap.MapId = dto.MapId;
                chap.ChapName = dto.ChapName;
                context.SubChapeters.Add(chap);
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
        public IActionResult DeleteChapters(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<SubChapeter> CurChaps = null;
            try
            {
                List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                CurChaps = (context.SubChapeters.Where(s => OrIds.Contains(s.ChapId.ToString()) == true).Select(s => s)).ToList();
                if (CurChaps != null)
                {
                    context.SubChapeters.RemoveRange(CurChaps);
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

        [HttpGet]
        public ActionResult GetChaprters(Guid MapId)//GetSubChapeterReqDTO dto)
        {
            List<SubChapeterDTO> Chaps = new List<SubChapeterDTO>();
            bool Success = false;
            try
            {
                Chaps = context.SubChapeters.Where(c => c.MapId == MapId)
                    .Select(s => new SubChapeterDTO { 
                    MapId=s.MapId,
                    ChapId=s.ChapId,
                    ChapName = s.ChapName
                    }).ToList();
                Success = true;
            }
            catch (Exception ex)
            {

            }
            if (Success)
            {
                return Ok(Chaps);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
