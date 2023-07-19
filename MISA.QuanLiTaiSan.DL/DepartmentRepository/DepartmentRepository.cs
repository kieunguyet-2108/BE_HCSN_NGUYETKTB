using Dapper;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.Enumeration;
using System.Data;
using MISA.QuanLiTaiSan.Common.UnitOfWork;

namespace MISA.QuanLiTaiSan.DL.DepartmentDL
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Thực hiện lấy ra thông tin entity theo điều kiện truyền vào
        /// </summary>
        /// <param name="condition">điều kiện</param>
        /// <returns>thông tin phòng ban</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public Department GetByCondition(string condition)
        {
            string procName = DatabaseUtility.GetProcdureName<Department>(MSProcdureName.GetByCondition);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("whereString", condition);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            return connection.QueryFirstOrDefault<Department>(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }
    }
}
