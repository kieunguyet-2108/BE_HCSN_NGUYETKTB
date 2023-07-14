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
        /// 
        /// </summary>
        /// <param name="fixedAsset"></param>
        /// <param name="action"></param>
        public void HandleActionOfAsset(VoucherDetail detail, Voucher voucher, int action);
    }
}
