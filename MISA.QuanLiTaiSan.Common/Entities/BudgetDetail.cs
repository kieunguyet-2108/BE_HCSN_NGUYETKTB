using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    public class BudgetDetail
    {

        public Guid budget_detail_id { get; set; } = Guid.NewGuid();

        public Guid budget_category_id { get; set; }
        public string budget_category_code { get; set; }

        public string budget_category_name { get; set; }

        public Guid fixed_asset_id { get; set; }

        public decimal budget_value { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string? modified_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? modified_date { get; set; }

        public int? action { get; set; }


    }
}
