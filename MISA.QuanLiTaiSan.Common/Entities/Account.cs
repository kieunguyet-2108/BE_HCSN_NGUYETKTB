using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{
    /// <summary>
    /// Lớp chứa thông tin tài khoản
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    [TableName("account")]
    public class Account
    {
        /// <summary>
        /// id của tài khoản
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public Guid account_id { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string account_name { get; set; }

        /// <summary>
        /// Email của tài khoản
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string phone_number { get; set; }

        /// <summary>
        /// Mật khẩy
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string password { get; set; }

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
    }
}
