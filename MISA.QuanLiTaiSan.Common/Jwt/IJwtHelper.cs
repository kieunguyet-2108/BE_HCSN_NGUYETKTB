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
        public string GenerateToken(Account account);
    }
}
