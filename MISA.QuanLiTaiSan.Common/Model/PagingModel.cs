using MISA.QuanLiTaiSan.Common.Entities;

namespace MISA.QuanLiTaiSan.Common.Model
{
    /// <summary>
    /// Lớp chứa thông tin phân trang sẽ trả về cho người dùng
    /// </summary> 
    /// Created By: NguyetKTB (20/05/2023)
    public class PagingModel<T>
    {

        #region Property
        /// <summary>
        /// Dữ liệu trả về bao gồm paging và filter
        /// </summary>  
        public IEnumerable<T>? Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int? TotalRecord { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// </summary>
        public object? Summary { get; set; }
        #endregion
    }
}
