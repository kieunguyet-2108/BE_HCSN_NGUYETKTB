using Dapper;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Entities;
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

namespace MISA.QuanLiTaiSan.DL.FixedAssetDL
{
    public class FixedAssetRepository : BaseRepository<FixedAsset>, IFixedAssetRepository
    {

        /// <summary>
        /// Hàm thực hiện lấy mã tài sản mới
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewCode()
        {
            string procName = "Proc_FixedAsset_GetNewCode";
            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<string>(procName, new { }, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Thực hiện lấy ra dữ liệu trả về thông qua paging và filter
        /// </summary>
        /// <param name="gridReader"></param>
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
        /// <param name="list"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public int InsertMultiple(List<FixedAsset> list)
        {
            var procName = "Proc_FixedAsset_InsertMultiple";
            var fixedAssetList = JsonConvert.SerializeObject(list);
            using (var connection = GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = connection.Execute(procName, new { FixedAssets = fixedAssetList }, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }

        }


        /// <summary>
        /// Lấy ra thông tin tài sản theo mã tài sản
        /// </summary>
        /// <param name="id">id của tài sản </param>
        /// <param name="code">mã tài sản </param>
        /// <returns>thông tin tài sản </returns>
        /// Created By: NguyetKTB (22/05/2023)
        public FixedAsset CheckExistOnInsert(string key, object value, string tableName)
        {

            string procName = DatabaseUtility.GetProcdureName<FixedAsset>(MSProcdureName.GetByCode);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"{key}", value);
            // tạo đối tượng kết nối với database
            using (var connection = GetConnection())
            {
                // thực hiện truy vấn dữ liệu
                return connection.QueryFirstOrDefault<FixedAsset>(procName, dynamicParameters, commandType: CommandType.StoredProcedure);
            }

        }

        /// <summary>
        /// Thực hiện kiểm tra tồn tại trong trường hợp update
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tableName"></param>
        /// <returns>true hoặc false</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public FixedAsset CheckExistOnUpdate(string key, object value, string tableName, Guid id)
        {
            string procName = DatabaseUtility.GetProcdureName<FixedAsset>(MSProcdureName.ExistUpdate);
            DynamicParameters dynamicParams = new DynamicParameters();
            dynamicParams.Add("code", value);
            dynamicParams.Add("id", id);
            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<FixedAsset>(procName, dynamicParams, commandType: CommandType.StoredProcedure);
            }
        }
    }
}


