using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    /// <summary>
    /// Lớp chi tiết chứng từ
    /// </summary> 
    /// Created By: NguyetKTB (20/06/2023)
    [TableName("voucher_detail")]
    public class VoucherDetail
    {
        /// <summary>
        /// Id của thông tin chứng từ
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        [PrimaryKey]
        public Guid voucher_detail_id { get; set; } = Guid.NewGuid();

        // <summary>
        /// Id của  chứng từ
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        public Guid? voucher_id { get; set; }

        // <summary>
        /// Id của tài sản thuộc chứng từ
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        public Guid? fixed_asset_id { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>  
        /// Created By: NguyetKTB (20/06/2023)
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày tạo
        /// Ngày tạo
        /// </summary> 
        /// Created By: NguyetKTB (20/06/2023)
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary> 
        /// Created By: NguyetKTB (20/06/2023)
        public string? modified_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>  
        /// Created By: NguyetKTB (20/06/2023)
        public DateTime? modified_date { get; set; }

    }
}
