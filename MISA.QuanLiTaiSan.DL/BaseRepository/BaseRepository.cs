using Dapper;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.DL.Database;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.BaseDL
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        #region DATABASE CONNECTION
        /// <summary>
        /// Thực hiện kết nối với database
        /// </summary>
        /// <returns>kết nối với database</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public MySqlConnection GetConnection()
        {
            // lấy ra chuỗi context kết nối tới database
            string connectonString = DatabaseContext.ConnectionString;
            // khởi tạo đối tượng kết nối
            MySqlConnection connection = new MySqlConnection(connectonString);
            // mở kết nối tới database
            connection.Open();
            return connection;
        }
        #endregion

        #region GET 
        /// <summary>
        /// Lấy ra danh sách dữ liệu
        /// </summary>
        /// <returns>Danh sách dữ liệu </returns>
        /// Created By: NguyetKTB (15/05/2023)
        public IEnumerable<T> GetList()
        {
            string procdureName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.GetAll);
            string a = DatabaseUtility.GetPrimaryKeyInTable<T>();
            using (var connection = GetConnection())
            {
                return connection.Query<T>(procdureName, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Lấy ra dữ liệu theo id
        /// </summary>
        /// <param name="id">Id cần lấy ra</param>
        /// <returns>Dữ liệu theo id truyền vào</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public T GetEntityById(Guid id)
        {
            string procedureName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.GetById);
            string primaryKey = DatabaseUtility.GetPrimaryKeyInTable<T>();
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"{primaryKey}", id);
            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<T>(procedureName, dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Update dữ liệu
        /// </summary>
        /// <param name="entity">dữ liệu update</param>
        /// <param name="entityId">id của entity sẽ update</param>
        /// <returns>số bản ghi được update</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Update(T entity, Guid entityId)
        {
            return 0;
        }
        #endregion

        #region INSERT
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu cần thêm mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Insert(T entity)
        {
            return 0;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="guids">danh sách dữ liệu cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Delete(Guid[] guids)
        {
            return 0;
        }
        #endregion
    }
}
