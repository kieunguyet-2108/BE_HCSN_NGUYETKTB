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
        /// Thực hiện xóa nhiều chi tiết nguồn chi phí theo trường được truyền vào
        /// </summary>
        /// <param name="ids">danh sách id chi tiết nguồn chi phí</param>
        /// <param name="field">trường muốn xóa theo</param>
        /// <returns>số bản ghi bị xóa</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int DeleteByField(string[] ids, string field);

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
