using EduManAPI.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.IO;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        EduManDBContext context = null;
        public FileManagerController(EduManDBContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public IActionResult SaveImageContent([FromForm]FileManagerInputDTO inp)
        {
            int bufferSize = 1000;
            int retval;
            int startIndex;
            byte[] inByte = new byte[bufferSize];
            if (inp.InpFile != null)
            {
                Stream readStream=inp.InpFile.OpenReadStream();
                MemoryStream ms = new MemoryStream();
                startIndex = 0;
                retval = readStream.Read(inByte, startIndex, bufferSize);
                while (retval == bufferSize)
                {
                    ms.Write(inByte);
                    inByte=new byte[bufferSize];
                    // Reposition start index to end of last buffer and fill buffer.  
                    startIndex += bufferSize;
                    readStream.Seek(startIndex, SeekOrigin.Begin);
                    retval = readStream.Read(inByte, 0, bufferSize);
                }
                // Write the remaining buffer.  
                ms.Write(inByte, 0, (int)retval);
                ms.Flush();
                byte[] SavedBytes= ms.ToArray();
                var DbConnection = context.Database.GetDbConnection();
                DbConnection.Open();
                FileDataInfo fi = SavedFileInfoHelper.GetInfo(inp.ReqDataIdentifer);
                DbCommand cmd = new SqlCommand($"UPDATE {fi.TblName} SET {fi.DataFldName}=@inpData, {fi.MimeFldName}='{inp.InpFile.ContentType}' where {fi.TblRowIdentiferColumn}='{inp.UniqIdentifer}'");
                DbParameter prm = new SqlParameter("@inpData", SqlDbType.Image, SavedBytes.Length);
                prm.Value = SavedBytes;
                cmd.Parameters.Add(prm);
                cmd.Connection= DbConnection;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return new JsonResult("Success....");
            }
            return BadRequest("error");
        }
       
        [HttpGet]
        public IActionResult GetImageContent([FromQuery]FileManagerInputDTO inp)
        {
            int bufferSize = 1024;
            long retval, startIndex;
            byte[] outByte = new byte[bufferSize];
            FileDataInfo fi = SavedFileInfoHelper.GetInfo(inp.ReqDataIdentifer);
            FileManagerDTO dto = null;
            if (fi!=null)
            {
               var DbConnection= context.Database.GetDbConnection();
                DbConnection.Open();
               DbCommand cmd = new SqlCommand($"Select * from {fi.TblName} where {fi.TblRowIdentiferColumn}='{inp.UniqIdentifer}'");
               cmd.Connection = DbConnection;
                MemoryStream ms = new MemoryStream();
                var reader= cmd.ExecuteReader();
                if (reader.Read())
                {
                    startIndex = 0;
                    retval = reader.GetBytes(fi.DataFldOrdinal, startIndex, outByte, 0, bufferSize);
                    // Continue while there are bytes beyond the size of the buffer.  
                    while (retval == bufferSize)
                    {
                        ms.Write(outByte);
                        ms.Flush();

                        // Reposition start index to end of last buffer and fill buffer.  
                        startIndex += bufferSize;
                        retval = reader.GetBytes(fi.DataFldOrdinal, startIndex, outByte, 0, bufferSize);
                    }
                    // Write the remaining buffer.  
                    ms.Write(outByte, 0, (int)retval);
                    ms.Flush();
                    

                    dto = new FileManagerDTO();
                    dto.Data = ms.ToArray();

                    dto.MimeType = reader[fi.MimeFldName].ToString();
                }
                reader.Close();
                DbConnection.Close();
                return Ok(dto);
            }
            return BadRequest("Eror In Processing request");
        }
    }
}
