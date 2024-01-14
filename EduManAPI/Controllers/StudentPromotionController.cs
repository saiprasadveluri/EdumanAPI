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
    public class StudentPromotionController : ControllerBase
    {
        EduManDBContext context = null;
        public StudentPromotionController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpDelete]
        public ActionResult UndoPromotion(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<StuStdAcdYearMap> CurMaps = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurMaps = (context.StuStdAcdYearMaps.Where(s => OrIds.Contains(s.MapId.ToString()) == true).Select(s => s)).ToList();
                if (CurMaps != null)
                {
                    context.StuStdAcdYearMaps.RemoveRange(CurMaps);
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
        public ActionResult GetPromotionList(Guid Stuid)
        {
            bool Success = false;
            StudentPromotionDataDTO dto = new StudentPromotionDataDTO();            
            try
            {
                var resobj = (from MapObj in context.StuStdAcdYearMaps
                              join StuObj in context.StudentInfos on MapObj.StuId equals StuObj.StuId
                              join StnObj in context.Standards on MapObj.StnId equals StnObj.StdId
                              join AcdObj in context.AcdYears on MapObj.AcYearId equals AcdObj.AcdId
                              where MapObj.StuId == Stuid && MapObj.RecType == 1
                              select new StudentPromotionDataEntryDTO() {
                                  MapId = MapObj.MapId,
                                  StuId = MapObj.StuId,
                                  StuName = StuObj.FName + "[" + StuObj.LName + "]",
                                  StnId = MapObj.StnId,
                                  StnName=StnObj.StdName,
                                  AcdYearId= AcdObj.AcdId,
                                  AcdYearName= AcdObj.AcdText,
                                  PromDate= MapObj.RecDate

                              }).ToList();
                dto.PromLst = resobj;
                Success = true;
            }
            catch (Exception ex)
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

        [HttpPost]
        public ActionResult PromoteStudents(StudentPromotionDTO dto)
        {
            bool Success = false;
            List<Guid> ErrorStuIds = new List<Guid>();
            try
            {
                if (dto.StuIdLst != null && dto.StuIdLst.Count > 0)
                {
                    foreach (var probj in dto.StuIdLst)
                    {
                        try
                        {
                            StuStdAcdYearMap ssmap = new StuStdAcdYearMap();
                            ssmap.MapId = Guid.NewGuid();
                            ssmap.StuId = probj;
                            ssmap.StnId = dto.ToStnId;
                            ssmap.AcYearId = dto.ToAcdYearId;

                            ssmap.RecDate = DateTime.Now;
                            ssmap.RecType = 1;//Promotion
                            context.StuStdAcdYearMaps.Add(ssmap);
                            context.SaveChanges();
                        }
                        catch (Exception exp)
                        {
                            ErrorStuIds.Add(probj);
                        }
                    }
                    Success = true;
                }
            }
            catch (Exception ex)
            { 
                
            }

            if (Success == true)
            {
                List<string> RegnNos = context.StudentInfos.Where(s => ErrorStuIds.Contains(s.StuId) == true).Select(s => s.RegdNo).ToList();
                return Ok(RegnNos);
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
