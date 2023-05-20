using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Exceptions
{
    public class MISAException : Exception
    {
        public string? ErrorMsg { get; set; }

        public Dictionary<string, string> ErrorData { get; set; }

        public MISAException(string errorMsg, Dictionary<string, string> errorData)
        {
            ErrorMsg = errorMsg;
            ErrorData = errorData;
        }

        public override string Message
        {
            get
            {
                return ErrorMsg;
            }
        }

        public override IDictionary Data
        {
            get
            {
                return ErrorData;
            }
        }

    }
}
