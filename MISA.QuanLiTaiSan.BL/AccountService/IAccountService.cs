using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.AccountService
{
    public interface IAccountService : IBaseService<Account>
    {
        /// <summary>
        /// Thực hiện kiểm tra thông tin đăng nhập có chính xác hay không
        /// </summary>
        /// <param name="loginModel">thông tin đăng nhập</param>
        /// <returns>token</returns>
        /// <exception cref="MISAException">lỗi trả về cho người dùng</exception>
        /// Created By: NguyetKTB (10/07/2023)
        public string Login(LoginModel loginModel);
    }
}
