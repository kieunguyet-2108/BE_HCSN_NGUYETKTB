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
        /// <summary>
        /// Tin nhắn lỗi gửi tới người dùng
        /// </summary>
        /// Created By: NguyetKTB (05/05/2023)
        public string? ErrorMsg { get; set; }

        /// <summary>
        /// Danh sách lỗi
        /// </summary>
        /// Created By: NguyetKTB (05/05/2023)
        public IDictionary? ErrorData { get; set; }

        /// <summary>
        /// Kết quả muốn gửi 
        /// </summary>
        /// Created By: NguyetKTB (05/05/2023)
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
