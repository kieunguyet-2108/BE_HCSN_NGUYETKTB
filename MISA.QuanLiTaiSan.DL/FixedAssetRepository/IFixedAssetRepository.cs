using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.Pagination;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.DL.FixedAssetDL
{
    public interface IFixedAssetRepository : IBaseRepository<FixedAsset>
    {
        /// <summary>
        /// Hàm thực hiện lấy mã tài sản mới
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewCode();

        /// <summary>
        /// Thực hiện insert nhiều tài sản
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public int InsertMultiple(List<FixedAsset> list);

        #region CHECK EXIST
        /// <summary>
        /// Thực hiện kiểm tra tồn tại trong trường hợp insert
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tableName"></param>
        /// <returns>true hoặc false</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public FixedAsset CheckExistOnInsert(string key, object value, string tableName);

        /// <summary>
        /// Thực hiện kiểm tra tồn tại trong trường hợp update
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tableName"></param>
        /// <returns>true hoặc false</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public FixedAsset CheckExistOnUpdate(string key, object value, string tableName, Guid id);

        #endregion
    }
}
