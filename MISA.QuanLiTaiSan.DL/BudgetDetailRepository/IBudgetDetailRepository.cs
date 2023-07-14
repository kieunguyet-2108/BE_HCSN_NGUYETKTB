using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.BudgetDetailRepository
{
    public interface IBudgetDetailRepository : IBaseRepository<BudgetDetail>
    {
        /// <summary>
        /// Thực hiện thêm mới nhiều bản ghi
        /// </summary>
        /// <param name="list">danh sách bản ghi cần thêm mới</param>
        /// <returns>số bản ghi được thêm mới</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int InsertMultiple(List<BudgetDetail> list);

        /// <summary>
        /// Thực hiện xóa nhiều bản ghi
        /// </summary>
        /// <param name="voucherIds">danh sách id chứng từ</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int DeleteByVouchers(string[] voucherIds);

        public int DeleteByAssets(string[] assetIds);

        /// <summary>
        /// Lấy ra danh sách thông tin chi tiết nguồn chi phí theo tài sản của chứng từ
        /// </summary>
        /// <param name="assetId">id của tài sản</param>
        /// <param name="voucherID">id của chứng từ</param>
        /// <returns>danh sách chi tiết chi phí</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public IEnumerable<BudgetDetail> GetByAsset(string assetId, string voucherID);
    }
}
