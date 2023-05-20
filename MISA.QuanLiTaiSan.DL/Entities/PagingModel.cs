namespace MISA.QuanLiTaiSan.Entities
{
    public class PagingModel<T> where T : class
    {
        /// <summary>
        /// Dữ liệu trả về bao gồm paging và filter
        /// </summary>  
        public IEnumerable<T>? Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int? TotalRecord { get; set; }
    }
}
