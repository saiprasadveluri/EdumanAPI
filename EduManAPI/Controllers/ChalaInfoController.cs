using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduManAPI.Infra;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class ChalaInfoController : ControllerBase
    {
        EduManDBContext context = null;
        public ChalaInfoController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetChalanList(Guid MapId)
        {
            List<ChalanListDTO> dto = new List<ChalanListDTO>();
            try
            {
                var lst = context.Chalans.Where(c => c.MapId == MapId && c.ChalanStatus== (int)ChalanStatusEnum.Active).ToList();
                foreach (var obj in lst)
                {
                    ChalanListDTO temp = new ChalanListDTO();
                    temp.ChlId = obj.ChlId;
                    temp.ChlDate = obj.ChlDate;
                    temp.ChlnNum = obj.ChlnNumber;
                    dto.Add(temp);
                }
            }
            catch (Exception exp)
            { 
            
            }
            return Ok(dto);
        }
    }
}
