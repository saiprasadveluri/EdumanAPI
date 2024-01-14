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
    
    [ApiController]
    [EnableCors("Policy1")]
    public class ChapterwiseController : ControllerBase
    {
        EduManDBContext context = null;
        public ChapterwiseController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult AddExam(ChaperExamAddDTO dto)
        {
            bool Success = false;
            string SavedFileName = string.Empty;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //Save the File content to folder...
                          SavedFileName= Guid.NewGuid().ToString();
                        bool SaveSuccess = SaveContentToFile(SavedFileName, dto.FileContentBase64);
                        if (SaveSuccess)
                        {
                            ChapterwiseExam exm = new ChapterwiseExam();
                            exm.ExamId = Guid.NewGuid();
                            exm.ExamDate = dto.ExamDate;
                            exm.Notes = dto.Notes;
                            exm.FileName = SavedFileName;
                            exm.MimeType = dto.MType;

                            context.ChatpterwiseExams.Add(exm);
                            context.SaveChanges();
                            Guid NewExamId = exm.ExamId;
                            foreach (var eobj in dto.entries)
                            {
                                ChapterwiseExamChapter sch = new ChapterwiseExamChapter();
                                sch.Id = Guid.NewGuid();
                                sch.ChapId = eobj.ChapId;
                                sch.ExamId = NewExamId;
                                context.ChapterwiseExamChapters.Add(sch);
                                context.SaveChanges();
                            }
                            transaction.Commit();
                            Success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if(SavedFileName.Length>0)
                            CleanAllSavedFiles(new List<string>() { SavedFileName });
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

        [Route("api/[controller]")]
        [HttpDelete]
        public ActionResult DeleteExams(string chks)
        {
            bool success = false;
            string ErrMsg = "";
            string[] lst = chks.Split(',');

            List<ChapterwiseExam> CurExams = null;
            try
            {
                List<string> OrIds = lst.ToList();//.Select(s => long.Parse(s)).ToList();
                CurExams = (context.ChatpterwiseExams.Where(s => OrIds.Contains(s.ExamId.ToString()) == true).Select(s => s)).ToList();
                
                //Delete rows
                if (CurExams != null)
                {
                    //Delete all attchments                
                    var SavedFileLst = CurExams.Select(e => e.FileName).ToList();
                    CleanAllSavedFiles(SavedFileLst);

                    context.ChatpterwiseExams.RemoveRange(CurExams);
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

        [Route("api/[controller]")]
        [HttpGet]
        public ActionResult GetChapterwiseExamDetails(Guid ExamId)
        {
            bool Success = false;
            List<ChapterwiseExamViewEntryDTO> dto = new List<ChapterwiseExamViewEntryDTO>();
            try
            {
                dto = (from obj in context.ChapterwiseExamChapters
                       join eobj in context.ChatpterwiseExams on obj.ExamId equals eobj.ExamId
                       join chobj in context.SubChapeters on obj.ChapId equals chobj.ChapId
                       join mobj in context.StdSubMaps on chobj.MapId equals mobj.MapId
                       join stobj in context.Standards on mobj.StdId equals stobj.StdId
                       join subobj in context.Subjects on mobj.SubId equals subobj.SubId
                       where obj.ExamId == ExamId
                       select new ChapterwiseExamViewEntryDTO()
                       {
                           ChapId=chobj.ChapId,
                           ChapName = chobj.ChapName,
                           StnName = stobj.StdName,
                           SubjectName = subobj.SubjectName,
                           
                       }).OrderByDescending(s =>s.StnName).ThenBy(s=>s.SubjectName).ThenBy(s=>s.ChapName).ToList();
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

        [Route("api/Chapterwise/{id}")]
        [HttpGet]
        public ActionResult GetChapterwiseExam(Guid id)
        {
            bool Success = false;
            List<ChapterwiseExamViewDTO> dto = new List<ChapterwiseExamViewDTO>();
            try
            {


                var ExamIds = (from obj in context.ChapterwiseExamChapters
                            join chobj in context.SubChapeters on obj.ChapId equals chobj.ChapId
                            join mobj in context.StdSubMaps on chobj.MapId equals mobj.MapId
                            join stobj in context.Standards on mobj.StdId equals stobj.StdId
                            join subobj in context.Subjects on mobj.SubId equals subobj.SubId
                            where stobj.OrgId == id
                            select obj.ExamId).Distinct().ToList();
                
                dto = (from obj in context.ChatpterwiseExams
                           where ExamIds.Contains(obj.ExamId)
                           select new ChapterwiseExamViewDTO
                           {
                               ExamId = obj.ExamId,
                               ExamDate = obj.ExamDate,
                               Notes = obj.Notes
                           }).ToList();

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
                var FilePath = Directory.GetCurrentDirectory() + "/ChapterExams/" + GuidFilenName;
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
                    var FilePath = Directory.GetCurrentDirectory() + "/ChapterExams/" + fn;
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
