using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Jwt
{
    public interface IJwtHelper
    {
        /// <summary>
        /// Thực hiện tạo token theo thông tin tài khoản truyền vào
        /// </summary>
        /// <param name="account">thông tin tài khoản</param>
        /// <returns>mã token</returns>
        /// Created By: NguyetKTB (25/06/2023)
        public string GenerateToken(Account account);
    }
}
