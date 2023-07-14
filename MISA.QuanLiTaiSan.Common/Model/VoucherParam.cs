using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Model
{
    public class VoucherParam
    {
        /// <summary>
        /// Thông tin chứng từ
        /// </summary>
        /// Created By: NguyetKTB (09/07/2023)
        public Voucher? Voucher { get; set; }

        /// <summary>
        /// Thông tin danh sách chi tiết của chứng từ
        /// </summary>
        /// Created By: NguyetKTB (09/07/2023)
        public List<FixedAsset>? FixedAssets{ get; set; }


        /// <summary>
        /// Thông tin danh sách nguồn hình thành của các tài sản
        /// </summary>
        /// Created By: NguyetKTB (09/07/2023)
        public List<BudgetDetail> BudgetDetails { get; set;}
    }
}
