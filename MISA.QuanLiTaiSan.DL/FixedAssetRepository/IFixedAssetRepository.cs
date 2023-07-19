using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.Pagination;
using static Dapper.SqlMapper;
using MISA.QuanLiTaiSan.Common.Model;

namespace MISA.QuanLiTaiSan.DL.FixedAssetDL
{
    public interface IFixedAssetRepository : IBaseRepository<FixedAsset>
    {


        /// <summary>
        /// Lấy ra danh sách tài sản theo chứng từ
        /// </summary>
        /// <param name="filterParam">thông tin phân trang, lock</param>
        /// <param name="whereCondition">điều kiện</param>
        /// <returns>
        /// Danh sách tài sản theo chứng từ, điều kiện lọc, phân trang
        ///</returns>
        /// Created By: NguyetKTB (05/07/2023)
        public PagingModel<FixedAsset> GetByVoucher(FilterParam filterParam, string whereCondition);


        /// <summary>
        /// Thực hiện insert nhiều tài sản
        /// </summary>
        /// <param name="list">danh sách tài sản cần thêm mới</param>
        /// <returns>tổng số tài sản được thêm mới</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public int InsertMultiple(List<FixedAsset> list);

        /// <summary>
        /// Thực hiện lấy ra các tài sản thuộc các chứng từ
        /// </summary>
        /// <param name="voucherIds">danh sách id của chứng từ</param>
        /// <returns>danh sách các tài sản thuộc chứng từ</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public IEnumerable<FixedAsset> GetFixedAssetsInVoucher(string voucherIds);


         /// <summary>
        /// Lấy ra danh sách tài sản tồn tại trong chứng từ
        /// </summary>
        /// <param name="assetIds">danh sách id tài sản</param>
        /// <returns>
        /// Danh sách tài sản
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public IEnumerable<FixedAsset> FindAssetInVoucher(string[] assetIds);



    }
}
