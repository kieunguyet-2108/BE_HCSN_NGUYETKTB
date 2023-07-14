using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Model
{
    /// <summary>
    /// Lớp chứa thông tin tóm tắt của bảng tài sản
    /// </summary> 
    /// Created By: NguyetKTB (25/05/2023)
    public class FixedAssetSummary
    {
        #region Property

        /// <summary>
        /// Tổng số bản ghi
        /// </summary> 
        /// Created By: NguyetKTB (25/05/2023)
        public int TotalRecord { get; set; }

        /// <summary>
        /// Tổng số lượng của tất cả các tài sản
        /// </summary> 
        /// Created By: NguyetKTB (25/05/2023)
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Tổng giá của tất cả các tài sản
        /// </summary> 
        /// Created By: NguyetKTB (25/05/2023)
        public decimal? TotalOfCost { get; set; }
        #endregion
    }
}
