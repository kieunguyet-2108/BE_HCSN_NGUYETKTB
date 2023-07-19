using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.AccountService;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Resources;
using System.Security.Principal;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class AccountController : BaseController<Account>
    {

        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) : base(accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Thực hiện lấy ra thông tin đăng nhập và trả về cho người dùng
        /// </summary>
        /// <param name="loginModel">thông tin đăng nhập, bao gồm tên và mật khẩu</param>
        /// <returns>
        /// 200 - thành công
        /// 400 - lỗi validate
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (05/07/2023)
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            IActionResult? actionResult = null;
            var account = _accountService.Login(loginModel);
            if (account != null)
            {
                actionResult = HandleResult("Đăng nhập thành công.", MSCODE.Success, account);
            }
            else
            {
                actionResult = HandleResult("Đăng nhập thất bại.", MSCODE.BadRequest, account);
            }
            return actionResult;
        }

    }
}
