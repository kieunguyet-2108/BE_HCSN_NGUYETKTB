using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Jwt;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.AccountRepository;
using MISA.QuanLiTaiSan.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.AccountService
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtHelper _jwtHepler;

        public AccountService(IAccountRepository accountRepository, IJwtHelper jwtHepler, IUnitOfWork unitOfWork) : base(accountRepository, unitOfWork)
        {
            _accountRepository = accountRepository;
            _jwtHepler = jwtHepler;
        }

        /// <summary>
        /// Thực hiện kiểm tra thông tin đăng nhập có chính xác hay không
        /// </summary>
        /// <param name="loginModel">thông tin đăng nhập</param>
        /// <returns>token</returns>
        /// <exception cref="MISAException">lỗi trả về cho người dùng</exception>
        /// Created By: NguyetKTB (10/07/2023)
        public string Login(LoginModel loginModel)
        {
            var account = _accountRepository.GetAccountByName(loginModel.user_name);
            if (account == null)
            {
                throw new MISAException("Tài khoản không tồn tại.");
            }
            else
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.password, account.password);
                if (!isValidPassword)
                {
                    throw new MISAException("Mật khẩu không hợp lệ");
                }
                else
                {
                    return _jwtHepler.GenerateToken(account);
                }
            }
        }

    }
}
