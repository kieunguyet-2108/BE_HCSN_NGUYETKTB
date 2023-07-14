using Dapper;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.Common.Utilities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.AccountRepository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Account GetAccountByName(string name)
        {
            string procedureName = "Proc_Account_GetByName";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("account_name", name);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            return connection.QueryFirstOrDefault<Account>(procedureName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }
    }
}
