using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class AppLinkDistributionController : ControllerBase
    {
        EduManDBContext context = null;
        private IConfiguration _config;
        public AppLinkDistributionController(EduManDBContext ctx, IConfiguration config)
        {
            context = ctx;
            _config = config;
        }

        [HttpPut]
        public ActionResult MarkAsDownLoaded(AppLinkDistributionEditEntryDTO dto)
        {
            bool Success = false;
            try
            {
                var rec = (from obj in context.AppLinkDistributions
                           where obj.StuMapId == dto.StuMapId
                           select obj).FirstOrDefault();
                if (rec != null)
                {
                    rec.Status = 3;//Downloaded..
                    context.SaveChanges();
                    Success = true;
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

                return BadRequest("Error");
            }
        }
        
        [HttpPost]
        public ActionResult MarkAsSent(List<Guid> StuMapIds)
        {
            bool Success = false;
            try
            {
                var ExtRecs = (from obj in context.AppLinkDistributions
                                 where StuMapIds.Contains(obj.StuMapId)
                                 select obj).ToList();

                var ExtmapIds = ExtRecs.Select(s => s.StuMapId).ToList();

                var NonExtmapIds = StuMapIds.Where(s => ExtmapIds.Contains(s)==false).ToList();
                //Update the status of existing records to SENT status
                foreach (var rec in ExtRecs)
                {
                    rec.Status = 2;
                }

                //Add new record to table if MapId does't exist.
                foreach (var mid in NonExtmapIds)
                {
                    AppLinkDistribution ad = new AppLinkDistribution();
                    ad.ALID = Guid.NewGuid();
                    ad.StuMapId = mid;
                    ad.Status = 2;
                    ad.Message = "Notification sent";
                    context.AppLinkDistributions.Add(ad);
                }
                context.SaveChanges();
                Success = true;
            }
            catch (Exception exp)
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

        [HttpGet]
        public ActionResult GetList(Guid StnId)
        {
            List<AppLinkDistributionEntryDTO> Lst = new List<AppLinkDistributionEntryDTO>();
            bool Success = false;
            try
            {
                Guid AcdYearId = Guid.Parse(_config["SiteSettings:AcdYearId"]);

                
                var ExtRecs = (from obj in context.AppLinkDistributions
                                 join mapObj in context.StuStdAcdYearMaps on obj.StuMapId equals mapObj.MapId
                                 join StnObj in context.Standards on mapObj.StnId equals StnObj.StdId
                                 join StuObj in context.StudentInfos on mapObj.StuId equals StuObj.StuId
                                 where StnObj.StdId == StnId
                                 select new { obj.StuMapId,obj.Status,StuObj.FName,StuObj.RegdNo}).ToList();
                
                
                var Temp1= (from obj in ExtRecs
                            select new AppLinkDistributionEntryDTO()
                            {
                                
                                StuMapId = obj.StuMapId,
                                Status = obj.Status,
                                StuName=obj.FName,
                                RegdNo= obj.RegdNo
                            }).ToList();

                var ExtMapIds = (from obj in Temp1
                                 select obj.StuMapId).ToList();

                var Temp2= (from obj in context.StuStdAcdYearMaps
                           join stuObj in context.StudentInfos on obj.StuId equals stuObj.StuId
                           where (obj.StnId==StnId && obj.AcYearId==AcdYearId && ExtMapIds.Contains(obj.MapId)==false)
                           select new AppLinkDistributionEntryDTO()
                           {
                               StuMapId = obj.MapId,
                               Status =1,
                               StuName = stuObj.FName,
                               RegdNo = stuObj.RegdNo
                           }).ToList();

                Lst = Temp1.Union(Temp2).ToList();

                Success = true;
            }
            catch (Exception exp)
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
