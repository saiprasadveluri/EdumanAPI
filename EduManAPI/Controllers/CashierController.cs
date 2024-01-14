using EduManAPI.Infra;
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
    public class CashierController : ControllerBase
    {
        EduManDBContext context = null;
        public CashierController(EduManDBContext ctx)
        {
            context = ctx;
        }
        
        [HttpPut]
        public ActionResult EditCashier(CashierDTO dto)
        {
            bool Success = false;
            try
            {
                var CurObj = (from obj in context.Cashiers
                              where obj.CashierId == dto.CashierId
                              select obj).FirstOrDefault();
                if (CurObj != null)
                {
                    CurObj.FName = dto.FName;
                    CurObj.LName = dto.LName;
                    CurObj.MName = dto.MName;
                    CurObj.DOJoining = dto.DOJoining;
                    CurObj.Status = dto.Status;
                    context.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception e)
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
        public ActionResult GetCashierList(Guid OrgId)
        {
            List<CashierDTO> dto = new List<CashierDTO>();
            try
            {
                dto = (from obj in context.Cashiers
                       join uobj in context.UserInfos on obj.LoginUID equals uobj.ID    
                       where obj.OrgId == OrgId
                           select new CashierDTO() {
                           CashierId= obj.CashierId,
                           OrgId=obj.OrgId,
                               EmpId= obj.EmpId,
                               FName=obj.FName,
                               MName=obj.MName,
                               LName=obj.LName,
                               DOJoining=obj.DOJoining,
                               Address=obj.Address,
                               MobileNo=uobj.MobNo,
                               Status = obj.Status
                           }).ToList();
                return Ok(dto);
            }
            catch (Exception ex)
            { 
                
            }
            return BadRequest("Error");
        }

        [HttpPost]
        public ActionResult AddCashier(CashierDTO dto)
        {
            bool Success = false;
            try
            {
                var RID = (from robj in context.Roles
                           where robj.RoleVal == (int)RoleEnum.Cashier
                           select robj.RoleID).FirstOrDefault();


                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //Add Login Info....
                        UserInfo uinfo = new UserInfo();
                        uinfo.ID = Guid.NewGuid();
                        uinfo.Name = dto.FName;
                        uinfo.MobNo = dto.MobileNo;
                        uinfo.Emailid = dto.EmailId; 
                        uinfo.Emailid = dto.EmpId;
                        uinfo.UserID = "CASH" + dto.EmpId;
                        uinfo.Password = "changeme";
                        context.UserInfos.Add(uinfo);
                        context.SaveChanges();

                        //Update User-Org-map table...
                        UserOrgMap umap = new UserOrgMap();
                        umap.MapId = Guid.NewGuid();
                        umap.OrgId = dto.OrgId;
                        umap.RoleId = RID;
                        umap.UserId = uinfo.ID;
                        context.UserOrgMaps.Add(umap);
                        context.SaveChanges();

                        Cashier cr = new Cashier();
                        cr.CashierId = Guid.NewGuid();
                        cr.OrgId = dto.OrgId;
                        cr.EmpId = dto.EmpId;
                        cr.FName = dto.FName;
                        cr.MName = dto.MName;
                        cr.LName = dto.LName;
                        cr.Address = dto.Address;                        
                        cr.DOJoining = dto.DOJoining;
                        cr.LoginUID = uinfo.ID;
                        cr.Status = 1;
                        context.Cashiers.Add(cr);
                        context.SaveChanges();
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
            if (Success)
            {
                return Ok("Success");
            }
            else {
                return BadRequest("Error");
            }

        }

        [HttpDelete]
        public IActionResult DeleteTeachers(string chks)
        {
            bool success = false;
            try
            {
                string[] lst = chks.Split(',');

                List<Cashier> CurCashiers = null;
                try
                {
                    List<string> OrIds = lst.ToList();//lst.Select(s => long.Parse(s)).ToList();
                    CurCashiers = (context.Cashiers.Where(s => OrIds.Contains(s.CashierId.ToString()) == true).Select(s => s)).ToList();
                    if (CurCashiers != null)
                    {
                        context.Cashiers.RemoveRange(CurCashiers);
                        context.SaveChanges();
                        success = true;
                    }
                }
                catch (Exception exp)
                {

                }
            }
            catch (Exception ex)
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
