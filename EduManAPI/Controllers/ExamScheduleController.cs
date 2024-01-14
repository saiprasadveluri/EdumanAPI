using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class ExamScheduleController : ControllerBase
    {
        EduManDBContext context = null;
        public ExamScheduleController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public ActionResult AddSchedule(ExamScheduleAddDTO dto)
        {
            bool Success = false;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    List<string> FileNames = new List<string>();
                    try
                    {
                        Exam exm = new Exam();
                        exm.ExamId = Guid.NewGuid();
                        exm.StartDate = dto.StartDate;
                        exm.EndDate = dto.EndDate;
                        exm.ExamTypeId = dto.ExamTypeId;
                        context.Exams.Add(exm);
                        context.SaveChanges();
                        Guid NewExamId = exm.ExamId;
                        foreach (var eobj in dto.entries)
                        {
                            //Save the File content to folder...
                            string FileName = Guid.NewGuid().ToString();
                            bool SaveSuccess = SaveContentToFile(FileName, eobj.FileBase64String);
                            if (SaveSuccess)
                            {
                                FileNames.Add(FileName);
                                ExamSchedule sch = new ExamSchedule();
                                sch.ExamSchId = Guid.NewGuid();
                                sch.MapId = eobj.MapId;
                                sch.ExamId = NewExamId;
                                sch.ExamDate = eobj.ExamDate;
                                sch.Notes = eobj.Notes;
                                sch.OrderNumber = eobj.OrdNo;
                                sch.FileName = FileName;
                                sch.MimeType = eobj.MimeType;
                                context.ExamSchedules.Add(sch);
                                context.SaveChanges();
                            }
                        }
                        transaction.Commit();
                        Success = true;
                    }
                    catch (Exception ex)
                    {
                        CleanAllSavedFiles(FileNames);
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception exp)
            { 
            
            }
            if (Success)
                return Ok("Success");
            else
                return BadRequest("Service Error");
        }

        [HttpPut]
        public ActionResult UpdateSchedule(ExamScheduleAddDTO dto)
        {
            bool Success = false;
            try
            {
                Exam exm = (from obj in context.Exams where dto.ExamId == obj.ExamId select obj).FirstOrDefault();
                if (exm != null)
                {
                    //Delete The Exam Scheduel and recreate the same
                    var Schs = context.ExamSchedules.Where(s => s.ExamId == dto.ExamId).Select(s => s).ToList();
                    context.ExamSchedules.RemoveRange(Schs);

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            exm.StartDate = dto.StartDate;
                            exm.EndDate = dto.EndDate;
                            exm.ExamTypeId = dto.ExamTypeId;
                            context.SaveChanges();
                            Guid NewExamId = exm.ExamId;
                            foreach (var eobj in dto.entries)
                            {
                                ExamSchedule sch = new ExamSchedule();
                                sch.ExamSchId = Guid.NewGuid();
                                sch.ExamId = NewExamId;
                                sch.ExamDate = eobj.ExamDate;
                                sch.Notes = eobj.Notes;
                                sch.OrderNumber = eobj.OrdNo;
                                context.ExamSchedules.Add(sch);
                                context.SaveChanges();
                            }
                            transaction.Commit();
                            Success = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception exp)
            {

            }
            if (Success)
                return Ok("Success");
            else
                return BadRequest("Service Error");
        }

        [HttpDelete]
        public ActionResult DeleteExams(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<Exam> CurExams = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                //Delete the Files associated with Selected exams
                var FileNames = (from obj in context.ExamSchedules
                                 join eobj in context.Exams on obj.ExamId equals eobj.ExamId
                                 where OrIds.Contains(eobj.ExamId.ToString())
                                 select obj.FileName).ToList();
                CleanAllSavedFiles(FileNames);              
                
                CurExams = (context.Exams.Where(s => OrIds.Contains(s.ExamId.ToString()) == true).Select(s => s)).ToList();
                if (CurExams != null)
                {
                    context.Exams.RemoveRange(CurExams);
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
                return BadRequest("Service Error");
            }
        }
        [HttpGet]
        public ActionResult GetExamSchedule(Guid ExamId)
        {
            bool Success = false;
            List<ExamScheduleViewEntryDTO> dto = new List<ExamScheduleViewEntryDTO>();
            try
            {
                dto = (from obj in context.ExamSchedules
                       join eobj in context.Exams on obj.ExamId equals eobj.ExamId
                       join tobj in context.ExamTypes on eobj.ExamTypeId equals tobj.ExamTypeId
                       join mobj in context.StdSubMaps on obj.MapId equals mobj.MapId
                       join stobj in context.Standards on mobj.StdId equals stobj.StdId
                       join subobj in context.Subjects on mobj.SubId equals subobj.SubId
                       where obj.ExamId == ExamId
                       select new ExamScheduleViewEntryDTO()
                       { 
                           ExmSchId=obj.ExamSchId,
                           StnId=stobj.StdId,
                           StnName = stobj.StdName,
                           SubjectName = subobj.SubjectName,
                           ExamDate= obj.ExamDate,
                           Notes=obj.Notes,
                           OrdNo = obj.OrderNumber
                       }).OrderByDescending(s=>s.OrdNo).ToList();
                Success = true;
            }
            catch (Exception exp)
            { 
            
            }
            if (Success)
            {
                return Ok(dto);
            }
            else
            {
                return BadRequest("Service error");
            }
        }

        private bool SaveContentToFile(string GuidFilenName, string Base64String)
        {
            bool Success = false;
            try
            {
                var FilePath = Directory.GetCurrentDirectory() + "/Exams/" + GuidFilenName;
                if (System.IO.File.Exists(FilePath))
                {
                    System.IO.File.Delete(FilePath);
                }

                //Save the Image File
                byte[] ImageBytes = Convert.FromBase64String(Base64String);
                FileStream fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
                fs.Write(ImageBytes, 0, ImageBytes.Length);//TODO: Check for File Length check
                fs.Close();
                Success = true;
            }
            catch (Exception exp)
            {

            }
            return Success;
        }

        private void CleanAllSavedFiles(List<string> FileNames)
        {
            foreach (var fn in FileNames)
            {
                try
                {
                    var FilePath = Directory.GetCurrentDirectory() + "/Exams/" + fn;
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(FilePath);
                    }
                }
                catch (Exception exp)
                { 
                    
                }
            }
        }
    }
}
