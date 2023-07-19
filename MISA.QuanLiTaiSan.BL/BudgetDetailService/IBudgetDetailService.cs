using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.BudgetDetailService
{
    public interface IBudgetDetailService : IBaseService<BudgetDetail>
    {
        /// <summary>
        /// Lấy ra danh sách thông tin chi tiết nguồn chi phí theo tài sản của chứng từ
        /// </summary>
        /// <param name="assetId">id của tài sản</param>
        /// <param name="voucherID">id của chứng từ</param>
        /// <returns>
        /// Danh sách thông tin chi tiết nguồn chi phí theo tài sản của chứng từ
        /// </returns>
        /// Created By: NguyetKTB (10/07/2023)
        public IEnumerable<BudgetDetail> GetByAsset(string assetId, string voucherID);

        /// <summary>
        /// Xử lý thông tin chi tiết nguồn chi phí theo hành động truyền vào
        /// </summary>
        /// <param name="budgetDetail">thông tin chi tiết nguồn chi phí</param>
        /// <param name="action">hành động</param>
        ///Created By: NguyetKTB (10/07/2023)
        public void HandleBudgetDetail(BudgetDetail budgetDetail, int action);
    }
}
