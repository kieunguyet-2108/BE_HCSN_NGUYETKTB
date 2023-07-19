using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    /// <summary>
    /// Bảng chứa thông tin chi tiết nguồn chi phí
    /// </summary>
    /// Created By: NguyetKTB (10/07/2023)
    [TableName("budget_detail")]
    public class BudgetDetail
    {
        /// <summary>
        /// id của chi tiết nguồn chi phí
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public Guid budget_detail_id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// id của nguồn chi phí
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public Guid budget_category_id { get; set; }

        /// <summary>
        /// Mã của nguồn chi phí
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public string budget_category_code { get; set; }

        /// <summary>
        /// Tên của nguồn chi phí
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public string budget_category_name { get; set; }

        /// <summary>
        /// Id tài sản thuộc nguồn chi phí
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Giá trị của tài sản thuộc nguồn chi phí 
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public decimal budget_value { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>  
        /// Created By: NguyetKTB (10/07/2023)
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary> 
        /// Created By: NguyetKTB (10/07/2023)
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary> 
        /// Created By: NguyetKTB (10/07/2023)
        public string? modified_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>  
        /// Created By: NguyetKTB (10/07/2023)
        public DateTime? modified_date { get; set; }

        /// <summary>
        /// Hành động của chi tiết nguồn chi phí
        /// </summary>
        /// Created By: NguyetKTB (10/07/2023)
        public int? action { get; set; }


    }
}
