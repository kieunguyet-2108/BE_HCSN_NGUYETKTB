using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    /// <summary>
    /// Lớp chứng từ
    /// </summary> 
    /// Created By: NguyetKTB (20/06/2023)
    [TableName("voucher")]
    public class Voucher
    {
        /// <summary>
        /// id của chứng từ
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        [PrimaryKey]
        public Guid voucher_id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// mã chứng từ
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        [Unique("Mã chứng từ đã tồn tại")]
        public string voucher_code { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        public DateTime voucher_date { get; set; }

        /// <summary>
        /// Ngày ghi tăng
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        public DateTime increment_date { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        public decimal total_orgprice { get; set; }

        /// <summary>
        /// Mô tả(ghi chú)
        /// </summary>
        /// Created By: NguyetKTB (20/06/2023)
        public string? description { get; set; }
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
