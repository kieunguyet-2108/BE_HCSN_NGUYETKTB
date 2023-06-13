using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Pagination
{
    /// <summary>
    /// Class này dùng để nhận các 
    /// điều kiện
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public class FilterCondition
    {
        #region Property
        /// <summary>
        /// Tên trường filter
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string? Field { get; set; }

        /// <summary>
        /// Giá trị filter
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string? Value { get; set; }

        /// <summary>
        /// Toán tử filter
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public int? Condition { get; set; }

        /// <summary>
        /// loại control
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public int? Operators { get; set; }
        #endregion
    }

    /// <summary>
    /// Class này dùng để nhận tham số gửi lên từ client
    /// để paging
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public class FilterParam
    {
        #region Property
        /// <summary>
        /// Số trang
        /// để paging
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public int PageNumber { get; set; }

        /// <summary>
        /// Số bản ghi trên 1 trang
        /// để paging
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public int PageSize { get; set; }

        /// <summary>
        /// Từ khóa search
        /// để paging
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string? Keyword { get; set; }

        /// <summary>
        /// Chứa thông tin filter 
        /// để paging
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public List<FilterCondition>? Filters { get; set; }
        #endregion
    }
}
