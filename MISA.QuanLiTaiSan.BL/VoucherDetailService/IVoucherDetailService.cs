using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.VoucherDetailService
{
    public interface IVoucherDetailService : IBaseService<VoucherDetail>
    {
        /// <summary>
        /// Xử lý thông tin chi tiết chứng từ theo hành động truyền vào
        /// </summary>
        /// <param name="detail">thông tin chi tiết chứng từ</param>
        /// <param name="voucher">thông tin chứng từ</param>
        /// <param name="action">hành động</param>
        /// Created By: NguyetKTB (20/06/2023)
        public void HandleActionOfAsset(VoucherDetail detail, Voucher voucher, int action);
    }
}
