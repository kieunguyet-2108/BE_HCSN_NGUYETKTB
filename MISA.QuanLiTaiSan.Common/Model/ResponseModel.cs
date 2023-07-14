using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Model
{
    /// <summary>
    /// Lớp bao gồm các thông tin sẽ được trả về cho người dùng
    /// </summary> 
    /// Created By: NguyetKTB (15/05/2023)
    public class ResponseModel
    {

        #region properties

        /// <summary>
        /// Thông báo cho dev
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string DevMsg { get; set; }

        /// <summary>
        /// Thông báo cho người dùng
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string UserMsg { get; set; }

        /// <summary>
        /// Dữ liệu muốn trả về
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public object? Data { get; set; }

        /// <summary>
        /// Thông báo lỗi muốn trả về (nếu có)
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public object? Error { get; set; }

        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public ResponseModel()
        {
        }
        public ResponseModel(string devMsg, string userMsg, object? data, object? error = null)
        {
            DevMsg = devMsg;
            UserMsg = userMsg;
            Data = data;
            Error = error;
        }
        #endregion

    }
}
