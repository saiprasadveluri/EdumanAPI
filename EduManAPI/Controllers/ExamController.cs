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
    public class ExamController : ControllerBase
    {
        EduManDBContext context = null;
        public ExamController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetExams(Guid OrgId)
        {
            bool Success = false;
            List<ExamViewDTO> dto = new List<ExamViewDTO>();

            try
            {
                dto = (from obj in context.Exams
                       join tobj in context.ExamTypes on obj.ExamTypeId equals tobj.ExamTypeId
                       where tobj.OrgId == OrgId
                       orderby obj.StartDate descending
                       select new ExamViewDTO()
                       {
                           ExamId = obj.ExamId,
                           ExamTypeString = tobj.TypeText,
                           StartDate = obj.StartDate,
                           EndDate = obj.EndDate
                       }).ToList();
                Success = true;
            }
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok(dto);
            }
            else
                return BadRequest("Service error");
        }
    }
}
