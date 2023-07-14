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

        public Account GetAccountByName(string name);
    }
}
