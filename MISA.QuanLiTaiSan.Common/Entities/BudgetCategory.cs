using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    /// <summary>
    /// Lớp nguồn hình thành
    /// </summary>  
    /// Created By: NguyetKTB (23/06/2023)  
    [TableName("budget_category")]
    public class BudgetCategory
    {
        /// <summary>
        /// id nguồn hình thành
        /// </summary>
        /// Created By: NguyetKTB (23/06/2023)  
        [PrimaryKey]
        public Guid budget_category_id { get; set; }

        /// <summary>
        /// Mã nguồn hình thành
        /// </summary>
        /// Created By: NguyetKTB (23/06/2023)  
        public string budget_category_code { get; set; }

        /// <summary>
        /// Tên nguồn hình thành
        /// </summary>
        /// Created By: NguyetKTB (23/06/2023)  
        public string budget_category_name { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>  
        /// Created By: NguyetKTB (23/06/2023)  
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary> 
        /// Created By: NguyetKTB (23/06/2023)  
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary> 
        /// Created By: NguyetKTB (23/06/2023)  
        public string? modified_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>  
        /// Created By: NguyetKTB (23/06/2023)  
        public DateTime? modified_date { get; set; }
    }
}
