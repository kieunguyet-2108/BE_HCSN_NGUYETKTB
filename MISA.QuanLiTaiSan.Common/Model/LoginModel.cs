using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Model
{
    public class LoginModel
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        /// Created By: NguyetKTB (25/06/2023)
        public string user_name {  get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        /// Created By: NguyetKTB (25/06/2023)
        public string password { get; set; }
    }
}
