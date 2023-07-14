using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Pagination;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.DL.BaseDL
{
    public interface IBaseRepository<T>
    {
        #region DATABASE CONNECTION
        /// <summary>
        /// Get database connection
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public DbConnection GetConnection();


        #endregion

        #region GET 
        /// <summary>
        /// Lấy ra danh sách dữ liệu
        /// </summary>
        /// <returns>Danh sách dữ liệu </returns>
        /// Created By: NguyetKTB (15/05/2023)
        public IEnumerable<T> GetList();

        /// <summary>
        /// Lấy ra dữ liệu theo id
        /// </summary>
        /// <param name="id">Id cần lấy ra</param>
        /// <returns>Dữ liệu theo id truyền vào</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public T GetEntityById(Guid id);

        /// <summary>
        /// lấy ra dữ liệu theo paging
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public PagingModel<T> GetByPaging(FilterParam filter, string? where);


        #endregion

        #region UPDATE
        /// <summary>
        /// Update dữ liệu
        /// </summary>
        /// <param name="entity">dữ liệu update</param>
        /// <param name="entityId">id của entity sẽ update</param>
        /// <returns>số bản ghi được update</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Update(T entity, Guid id);
        #endregion

        #region INSERT
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu cần thêm mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Insert(T entity);
        #endregion

        #region DELETE
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="guids">danh sách dữ liệu cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Delete(string[] id);


        #endregion

        /// <summary>
        /// Hàm thực hiện lấy mã tài sản mới
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewCode();

        public T CheckDuplicate(string proValue, object? id);


    }
}
