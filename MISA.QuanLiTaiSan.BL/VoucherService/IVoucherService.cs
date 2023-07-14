using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.VoucherService
{
    public interface IVoucherService : IBaseService<Voucher>
    {

        /// <summary>
        /// Thực hiện thêm chứng từ 
        /// </summary>
        /// <param name="voucher">thông tin chứng từ</param>
        /// <param name="FixedAssets">thông tin tài sản thuộc chứng từ</param>
        /// <param name="BudgetDetails">thông tin nguồn chi phí</param>
        /// <returns>chứng từ đã được thêm mới thành công</returns>
        /// Created By: NguyetKTB (09/07/2023)
        public Voucher InsertVoucher(Voucher voucher, List<FixedAsset>? FixedAssets, List<BudgetDetail>? BudgetDetails);

        /// <summary>
        /// Thực hiện cập nhật thông tin chứng từ
        /// </summary>
        /// <param name="voucher">thông tin chứng từ</param>
        /// <param name="FixedAssets">thông tin tài sản thuộc chứng từ</param>
        /// <param name="BudgetDetails">thông tin nguồn chi phí</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int UpdateVoucher(Voucher voucher, List<FixedAsset>? FixedAssets, List<BudgetDetail>? BudgetDetails);
    }
}
