using Dapper;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.Pagination;
using System.Reflection.PortableExecutable;
using static Dapper.SqlMapper;
using MISA.QuanLiTaiSan.Common.Utilities;
using System.Reflection;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using MISA.QuanLiTaiSan.Common.Model;
using Newtonsoft.Json.Linq;
using MISA.QuanLiTaiSan.Common.UnitOfWork;

namespace MISA.QuanLiTaiSan.DL.FixedAssetDL
{
    public class FixedAssetRepository : BaseRepository<FixedAsset>, IFixedAssetRepository
    {

        public FixedAssetRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }



        /// <summary>
        /// Thực hiện lấy ra dữ liệu trả về thông qua paging và filter
        /// </summary>
        /// <param name="gridReader">grid reader</param>
        /// <returns>
        /// model bao gồm data(dữ liệu trả về), tổng số bản ghi và summary của table
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public override PagingModel<FixedAsset> HandlePagingModel(GridReader gridReader)
        {
            var data = gridReader.Read<FixedAsset>();
            var summary = gridReader.Read<FixedAssetSummary>().FirstOrDefault();
            return new PagingModel<FixedAsset>()
            {
                Data = data,
                Summary = summary,
                TotalRecord = ((FixedAssetSummary)summary).TotalRecord,
            };
        }

        /// <summary>
        /// Thực hiện insert nhiều tài sản
        /// </summary>
        /// <param name="list">danh sách tài sản cần thêm mới</param>
        /// <returns>tổng số tài sản được thêm mới</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public int InsertMultiple(List<FixedAsset> list)
        {
            var procName = "Proc_FixedAsset_InsertMultiple";
            var fixedAssetList = JsonConvert.SerializeObject(list);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            try
            {
                var result = connection.Execute(procName, new { FixedAssets = fixedAssetList }, commandType: CommandType.StoredProcedure, transaction: transaction);
                return result;
            }
            catch 
            {
                throw;
            }
        }


        /// <summary>
        /// Lấy ra danh sách tài sản theo chứng từ
        /// </summary>
        /// <param name="filterParam">thông tin phân trang, lock</param>
        /// <param name="whereCondition">điều kiện</param>
        /// <returns>
        /// Danh sách tài sản theo chứng từ, điều kiện lọc, phân trang
        ///</returns>
        /// Created By: NguyetKTB (05/07/2023)
        public PagingModel<FixedAsset> GetByVoucher(FilterParam filterParam, string whereCondition)
        {
            string procName = DatabaseUtility.GetProcdureName<FixedAsset>(MSProcdureName.GetByVoucher);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            var result = connection.QueryMultiple(procName,
            new
            {
                ms_offset = (filterParam.PageNumber - 1) * filterParam.PageSize,
                ms_limit = filterParam.PageSize,
                ms_where = whereCondition
            }, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
            return HandlePagingModel(result);
        }

        /// <summary>
        /// Thực hiện lấy ra các tài sản thuộc các chứng từ
        /// </summary>
        /// <param name="id">danh sách id của tài sản</param>
        /// <returns>danh sách các tài sản thuộc chứng từ</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public IEnumerable<FixedAsset> GetFixedAssetsInVoucher(string id)
        {
            string procName = DatabaseUtility.GetProcdureName<FixedAsset>(MSProcdureName.GetListInVoucher);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("voucher_id", id);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            return connection.Query<FixedAsset>(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        /// <summary>
        /// Lấy ra danh sách tài sản tồn tại trong chứng từ
        /// </summary>
        /// <param name="assetIds">danh sách id tài sản</param>
        /// <returns>
        /// Danh sách tài sản
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public IEnumerable<FixedAsset> FindAssetInVoucher(string[] assetIds)
        {
            string procName = DatabaseUtility.GetProcdureName<FixedAsset>(MSProcdureName.FindByVoucher);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ids", string.Join(",", assetIds));
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            return connection.Query<FixedAsset>(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

    }
}


