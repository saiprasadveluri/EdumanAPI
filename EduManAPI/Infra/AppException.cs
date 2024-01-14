using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Infra
{
    public class AppException:ApplicationException
    {
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }

        public AppException(int Ecode, string Emsg)
        {
            ErrorCode = Ecode;
            ErrorMsg = Emsg;
        }
    }
}
