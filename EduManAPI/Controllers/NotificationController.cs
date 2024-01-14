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
    public class NotificationController : ControllerBase
    {
        EduManDBContext context = null;
        public NotificationController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ActionResult GetNotifications(Guid OrgId, int ReqType, Guid RequesterId,Guid AcdYear,int SentItems)
        {
            bool Success = false;
            List<NotificationViewDTO> dto = new List<NotificationViewDTO>();
            if (SentItems == 0)
            {
                try
                {
                    if (ReqType == 1)//Student
                    {
                        var ResAllOrAllStu = (from obj in context.Notifications
                                              join robj in context.NotificationRecps on obj.NID equals robj.NID
                                              join fobj in context.UserInfos on obj.From equals fobj.ID
                                              where obj.OrgId == OrgId && robj.RecpType == 1 || robj.RecpType == 2
                                              select new NotificationViewDTO()
                                              {
                                                  NID = obj.NID,
                                                  From = fobj.Name,
                                                  NDate = obj.NDate,
                                                  NSubject = obj.NSubject,
                                                  NBody = obj.NBody
                                              }).ToList();

                        var ResIndvCalss = (from obj in context.Notifications
                                            join robj in context.NotificationRecps on obj.NID equals robj.NID
                                            join mobj in context.StuStdAcdYearMaps on robj.RecpId equals mobj.StnId
                                            join Stuinfo in context.StudentInfos on mobj.StuId equals Stuinfo.StuId
                                            join fobj in context.UserInfos on obj.From equals fobj.ID
                                            where (obj.OrgId == OrgId) && (robj.RecpType == 6 && Stuinfo.LoginUID == RequesterId && mobj.AcYearId == AcdYear)
                                            select new NotificationViewDTO()
                                            {
                                                NID = obj.NID,
                                                From = fobj.Name,
                                                NDate = obj.NDate,
                                                NSubject = obj.NSubject,
                                                NBody = obj.NBody
                                            }).ToList();

                        var ResIndvStudent = (from obj in context.Notifications
                                              join robj in context.NotificationRecps on obj.NID equals robj.NID
                                              join fobj in context.UserInfos on obj.From equals fobj.ID
                                              where (obj.OrgId == OrgId) && (robj.RecpType == 4 && robj.RecpId == RequesterId)
                                              select new NotificationViewDTO()
                                              {
                                                  NID = obj.NID,
                                                  From = fobj.Name,
                                                  NDate = obj.NDate,
                                                  NSubject = obj.NSubject,
                                                  NBody = obj.NBody
                                              }).ToList();

                        dto = ResAllOrAllStu.Union(ResIndvCalss).Union(ResIndvStudent).ToList();
                    }
                    else if (ReqType == 2)
                    {          //Teacher 
                        var ResAllOrAllTeach = (from obj in context.Notifications
                                                join robj in context.NotificationRecps on obj.NID equals robj.NID
                                                join fobj in context.UserInfos on obj.From equals fobj.ID
                                                where (obj.OrgId == OrgId) && (robj.RecpType == 1 || robj.RecpType == 3)
                                                select new NotificationViewDTO()
                                                {
                                                    NID = obj.NID,
                                                    From = fobj.Name,
                                                    NDate = obj.NDate,
                                                    NSubject = obj.NSubject,
                                                    NBody = obj.NBody
                                                }).ToList();

                        var ResIndvTeach = (from obj in context.Notifications
                                            join robj in context.NotificationRecps on obj.NID equals robj.NID
                                            join fobj in context.UserInfos on obj.From equals fobj.ID
                                            where (obj.OrgId == OrgId) && (robj.RecpType == 5 && robj.RecpId == RequesterId)
                                            select new NotificationViewDTO()
                                            {
                                                NID = obj.NID,
                                                From = fobj.Name,
                                                NDate = obj.NDate,
                                                NSubject = obj.NSubject,
                                                NBody = obj.NBody
                                            }).ToList();
                        dto = ResAllOrAllTeach.Union(ResIndvTeach).ToList();
                    }
                    Success = true;
                }
                catch (Exception ex)
                {

                }
            }
            else if (SentItems == 1)
            {
                try
                {
                    var temp1 = (from obj in context.Notifications
                                 join robj in context.NotificationRecps on obj.NID equals robj.NID
                                 where obj.From == RequesterId
                                 select new NotificationSentTempDTO()
                                 {
                                     NID = obj.NID,
                                     //From = fobj.Name,
                                     NDate = obj.NDate,
                                     NSubject = obj.NSubject,
                                     NBody = obj.NBody,
                                     RecpType = robj.RecpType,
                                     RecId = robj.RecpId
                                 }).ToList();
                    var UniqeNIds = temp1.Distinct(new NotificationSentItemsComparer()).ToList();
                    if (UniqeNIds.Count > 0)
                    {
                        for (int n = 0; n < UniqeNIds.Count; ++n)
                        {
                            var CurItem = UniqeNIds[n];
                            
                            var temp2 = (from obj in temp1
                                         where obj.NID == CurItem.NID
                                         select obj).ToList();

                            var ToLstString = GetRecpListString(temp2, OrgId);
                            
                            NotificationViewDTO res = new NotificationViewDTO() { 
                            
                             NID=CurItem.NID,
                             NDate=CurItem.NDate,
                             From=CurItem.From,
                             NSubject=CurItem.NSubject,
                             NBody=CurItem.NBody,
                             ToLst=ToLstString
                            };

                            dto.Add(res);
                        }
                    }
                    Success = true;
                }
                catch (Exception exp)
                { 
                    
                }
                
            }
            if (Success)
                return Ok(dto);
            else
                return BadRequest("Error");
        }

        [HttpPost]
        public ActionResult AddNotification(NotificationDTO dto)
        {
            bool Success = false;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Notification n = new Notification();
                        n.NID = Guid.NewGuid();
                        n.From = dto.From;
                        n.NBody = dto.NBody;
                        n.NDate = dto.NDate;
                        n.NSubject = dto.NSubject;
                        n.OrgId = dto.OrgId;
                        context.Notifications.Add(n);
                        context.SaveChanges();
                        foreach (var rec in dto.Recps)
                        {
                            NotificationRecp r = new NotificationRecp();
                            r.RID = Guid.NewGuid();
                            r.NID = n.NID;
                            r.RecpType = rec.RecpType;
                            r.RecpId = rec.RecpId;
                            context.NotificationRecps.Add(r);
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
            catch (Exception e)
            { 
            
            }
            if (Success)
            {
                return Ok("Success");
            }
            else
                return BadRequest("Error");
        }

        private string GetRecpListString(List<NotificationSentTempDTO> inp,Guid OrgId)
        {
            string ResString = "";
            
            foreach (var itm in inp)
            {
                if (itm.RecpType == (int)NotificationReceipienstEnum.All)
                {
                    ResString += "All,";
                }
                if (itm.RecpType == (int)NotificationReceipienstEnum.AllTeachers)
                {
                    ResString += "All Teachers,";
                }
                if (itm.RecpType == (int)NotificationReceipienstEnum.AllStudents)
                {
                    ResString += "All Students,";
                }
                if (itm.RecpType == (int)NotificationReceipienstEnum.IndividualClass)
                {
                    var ClassName = (from obj in context.Standards
                                     where obj.OrgId == OrgId && obj.StdId == itm.RecId
                                     select obj.StdName).FirstOrDefault();
                    ResString += "Individual Class ["+ClassName+"],";
                }
                if (itm.RecpType == (int)NotificationReceipienstEnum.IndividualTeacher)
                {
                    var Name = (from obj in context.Teachers
                                     join uinfo in context.UserInfos on obj.LoginUID equals uinfo.ID
                                     where uinfo.ID == itm.RecId
                                     select obj.FName).FirstOrDefault();
                    ResString += "Teacher [" + Name + "],";
                }
                if (itm.RecpType == (int)NotificationReceipienstEnum.IndividualStudent)
                {
                    var Name = (from obj in context.StudentInfos
                                join uinfo in context.UserInfos on obj.LoginUID equals uinfo.ID
                                where uinfo.ID == itm.RecId
                                select obj.FName).FirstOrDefault();
                    ResString += "Student [" + Name + "],";
                }
            }
            return ResString;
        }
    }
}
