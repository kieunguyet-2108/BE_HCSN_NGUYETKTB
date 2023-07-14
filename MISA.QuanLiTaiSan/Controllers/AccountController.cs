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
        [HttpGet("GeneratePassword")]
        public IActionResult GeneratePassword(string password)
        {
            password = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(password);
        }

        [HttpGet("VertifyPassword")]
        public IActionResult VertifyPassword(string password, string hashedPassword)
        {
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return Ok(validPassword);
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            IActionResult? actionResult = null;
            var account = _accountService.Login(loginModel);
            actionResult = HandleResult("Đăng nhập thành công.", MSCODE.Success, account);
            return actionResult;
        }

    }
}
