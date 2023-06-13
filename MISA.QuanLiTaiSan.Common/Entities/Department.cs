using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    /// <summary>
    /// Lớp phòng ban
    /// </summary>  
    /// Created By: NguyetKTB (15/05/2023)  
    [TableName("department")]
    public class Department
    {
        #region Properties
        /// <summary>
        /// Id phòng ban
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        [PrimaryKey]
        public Guid department_id { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string department_code { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string department_name { get; set; }

        /// <summary>
        /// Mô tả của ph
        /// òng ban
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string? description { get; set; }

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
        #endregion
    }
}
