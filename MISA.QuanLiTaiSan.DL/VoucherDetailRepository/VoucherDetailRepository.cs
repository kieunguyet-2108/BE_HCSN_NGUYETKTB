using Dapper;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.DL.VoucherRepository;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.VoucherDetailRepository
{
    public class VoucherDetailRepository : BaseRepository<VoucherDetail>, IVoucherDetailRepository
    {
        public VoucherDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Thực hiện xóa nhiều chi tiết chứng từ theo trường được truyền vào
        /// </summary>
        /// <param name="ids">danh sách id chi tiết chứng từ</param>
        /// <param name="field">trường muốn xóa theo</param>
        /// <returns>số bản ghi bị xóa</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int DeleteByField(string[] ids, string field)
        {
            string procName = "Proc_VoucherDetail_DeleteByField";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ids", string.Join(",", ids));
            dynamicParameters.Add("deleteBy", field);
            var connection = _unitOfWork.GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            try
            {
                var rowEffects = connection.Execute(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                return rowEffects;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Thực hiện insert nhiều tài sản
        /// </summary>
        /// <param name="list">danh sách chi tiết chứng từ cần thêm mới</param>
        /// <returns>số bản ghi được thêm mới</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public int InsertMultiple(List<VoucherDetail> list)
        {
            var procName = "Proc_VoucherDetail_InsertMultiple";
            var voucherDetails = JsonConvert.SerializeObject(list);
            var connection = GetConnection();
            try
            {
                var result = connection.Execute(procName, new { VoucherDetails = voucherDetails }, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.GetTransaction());
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
