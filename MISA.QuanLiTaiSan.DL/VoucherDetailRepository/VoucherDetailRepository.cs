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
        /// Thực hiện xóa nhiều tài sản thuộc chứng từ theo id được truyền vào
        /// </summary>
        /// <param name="voucherIds"></param>
        /// <returns>số bản ghi đã được xóa</returns>
        /// Created By: NguyetKTB (25/06/2023)
        public int DeleteMultiple(string[] vourcherIds, string field)
        {
            string procName = DatabaseUtility.GetProcdureName<VoucherDetail>(MSProcdureName.DeleteByVoucher);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ids", string.Join(",", vourcherIds));
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

        public int DeleteByAsset(string[] assetIds)
        {
            string procName = "Proc_VoucherDetail_DeleteByAsset";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("assetIds", string.Join(",", assetIds));
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
        /// <param name="list"></param>
        /// <returns></returns>
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
