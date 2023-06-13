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

        public IDictionary? ErrorData { get; set; }

        public object? Result { get; set; }

        public MISAException(string errorMsg, IDictionary? errorData = null, object? result = null)
        {
            ErrorMsg = errorMsg;
            ErrorData = errorData;
            Result = result;
        }

        public override string? Message
        {
            get
            {
                return ErrorMsg;
            }
        }

        public override IDictionary? Data
        {
            get
            {
                return ErrorData;
            }
        }

    }
}
