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
    public class CoCurricularMarkController : ControllerBase
    {
        EduManDBContext context = null;
        public CoCurricularMarkController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetCoCurMarks(Guid ExamID, Guid MapID)
        {
            bool Success = false;
            CoCurricularMarkDTO dto = new CoCurricularMarkDTO();
            try
            {
               dto = (from obj in context.CoCurricularMarks
                      where obj.ExamId==ExamID && obj.StuMapId==MapID
                      select new CoCurricularMarkDTO { 
                      CCID=obj.CCID,
                      CulturalEducation= obj.CulturalEducation,
                      ComputerEducation=obj.ComputerEducation,
                      PhysicalEducation=obj.PhysicalEducation,
                      ValueEducation=obj.ValueEducation,
                      OtherAreas = obj.OtherAreas
                      }).FirstOrDefault();

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
        public ActionResult AddCoCurMarks(List<CoCurricularMarkDTO> dto)
        {
            bool Success = false;

            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var entry in dto)
                        {
                            CoCurricularMark cc = new CoCurricularMark();
                            cc.CCID = Guid.NewGuid();
                            cc.ExamId = entry.ExamId;
                            cc.StuMapId = entry.StuMapId;
                            cc.ComputerEducation = entry.ComputerEducation;
                            cc.CulturalEducation = entry.CulturalEducation;
                            cc.PhysicalEducation = entry.PhysicalEducation;
                            cc.ValueEducation = entry.ValueEducation;
                            cc.OtherAreas = entry.OtherAreas;
                            context.CoCurricularMarks.Add(cc);
                            context.SaveChanges();
                        }
                        
                        transaction.Commit();
                        Success = true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
            if (Success == true)
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
