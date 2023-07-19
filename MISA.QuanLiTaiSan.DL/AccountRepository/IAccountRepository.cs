using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.AccountRepository
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        /// <summary>
        /// Lấy ra tài khoản theo tên đăng nhập
        /// </summary>
        /// <param name="name">tên đăng nhập</param>
        /// <returns>
        /// Tài khoản
        /// </returns>
        /// Created By: NguyetKTB (20/06/2023)
        public Account GetAccountByName(string name);
    }
}
