using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Model;

namespace MISA.QuanLiTaiSan.Common.Jwt
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Thực hiện generate token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="MISAException">exception trả về lỗi</exception>
        public string GenerateToken(Account account)
        {
            // đọc cấu hình từ appsetting.json
            var jwtKey = _configuration["Jwt:Key"];
            var jwtExpireMinutes = _configuration["Jwt:ExpireMinutes"];
            // thực hiện kiểm tra cấu hình
            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtExpireMinutes))
            {
                throw new MISAException(Resources.ResourceVN.Msg_Exception);
            }
            // tạo đối tượng để xử lí token
            var tokenHandler = new JwtSecurityTokenHandler();
            //
            var secretKey = Encoding.ASCII.GetBytes(jwtKey); // get secret key from appsettings.json
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.account_id.ToString()),
                    new Claim(ClaimTypes.Name, account.account_name.ToString()),
                    new Claim(ClaimTypes.Email, account.email),
                    new Claim(ClaimTypes.MobilePhone, account.phone_number),
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtExpireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
