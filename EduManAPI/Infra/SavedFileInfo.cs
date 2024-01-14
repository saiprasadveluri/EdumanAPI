using System;
using System.Collections.Generic;
using System.Linq;

namespace EduManAPI.Infra
{
    public static class SavedFileInfoHelper
    {
        private static List<FileDataInfo> _files = new List<FileDataInfo>()
        {
            new FileDataInfo()
            {
                Id=1,
                TblName="StudentInfos",                
                DataFldOrdinal=24,
                DataFldName="StudentImage",
                MimeFldName="StudentImageMimeType",
                TblRowIdentiferColumn="StuId"
            }
        };

        public static FileDataInfo GetInfo(int ReqId)
        {
            FileDataInfo reqFileInfo = _files.Where(f => f.Id == ReqId).FirstOrDefault();
            return reqFileInfo;
        }
    }

    public class FileDataInfo
    {
        public int Id { get; set; }
        public string TblName { get; set; }
        public string DataFldName { get; set; }
        public int DataFldOrdinal { get; set; }
        public string MimeFldName { get; set; }
        public string TblRowIdentiferColumn { get; set; } 

    }
}
