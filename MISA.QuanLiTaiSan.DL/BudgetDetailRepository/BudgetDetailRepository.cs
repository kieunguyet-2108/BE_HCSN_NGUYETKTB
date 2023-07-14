using Dapper;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.BudgetCategoryRepository;
using MISA.QuanLiTaiSan.DL.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.BudgetDetailRepository
{
    public class BudgetDetailRepository : BaseRepository<BudgetDetail>, IBudgetDetailRepository
    {
        public BudgetDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Thực hiện thêm mới nhiều bản ghi vào database
        /// </summary>
        /// <param name="list">danh sách dữ liệu sẽ thêm mới</param>
        /// <returns>số bản ghi được thêm mới</returns>
        /// Created By: NguyetKTB (25/06/2023)
        public int InsertMultiple(List<BudgetDetail> list)
        {
            var procName = "Proc_BudgetDetail_InsertMultiple";
            var budgetDetails = JsonConvert.SerializeObject(list);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            try
            {
                var result = connection.Execute(procName, new { BudgetDetails = budgetDetails }, commandType: CommandType.StoredProcedure, transaction:transaction);
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Thực hiện xóa nhiều tài sản thuộc chứng từ theo id được truyền vào
        /// </summary>
        /// <param name="voucherIds"></param>
        /// <returns>số bản ghi đã được xóa</returns>
        /// Created By: NguyetKTB (25/06/2023)
        public int DeleteByVouchers(string[] vourcherIds)
        {
            string procName = DatabaseUtility.GetProcdureName<BudgetDetail>(MSProcdureName.DeleteByVoucher);
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

        public int DeleteByAssets(string[] assetIds)
        {
            string procName = "Proc_BudgetDetail_DeleteByAsset";
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
        /// Lấy ra danh sách thông tin chi tiết nguồn chi phí theo tài sản của chứng từ
        /// </summary>
        /// <param name="assetId">id của tài sản</param>
        /// <param name="voucherID">id của chứng từ</param>
        /// <returns>danh sách chi tiết chi phí</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public IEnumerable<BudgetDetail> GetByAsset(string assetId, string voucherID)
        {
            string procName = "Proc_BudgetDetail_GetByAsset";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("fixed_asset_id", assetId);
            dynamicParameters.Add("voucher_id", voucherID);
            var connection = _unitOfWork.GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            try
            {
                return connection.Query<BudgetDetail>(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            catch
            {
                throw;
            }
        }

        public override int Delete(string[] id)
        {
            string procName = DatabaseUtility.GetProcdureName<BudgetDetail>(MSProcdureName.DeleteById);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ids", string.Join(",", id));
            var connection = GetConnection();
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
    }
}
