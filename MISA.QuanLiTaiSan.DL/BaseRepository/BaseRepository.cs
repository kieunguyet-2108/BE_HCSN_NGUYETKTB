using Dapper;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.Utilities;
using MISA.QuanLiTaiSan.Entities;
using MISA.QuanLiTaiSan.Common.Pagination;
using System.Drawing;
using static Dapper.SqlMapper;
using System.Reflection.PortableExecutable;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Resources;

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
            string primaryKey = AttributeUtility.GetPrimaryKeyName<T>();
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"{primaryKey}", id);
            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<T>(procedureName, dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Created By: NguyetKTB (15/05/2023) 
        public virtual PagingModel<T> GetByPaging(FilterParam filter, string? where)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.GetByPaging);
            using (var connection = GetConnection())
            {
                var result = connection.QueryMultiple(procName,
                new
                {
                    ms_offset = (filter.PageNumber - 1) * filter.PageSize,
                    ms_limit = filter.PageSize,
                    ms_where = where
                }, commandType: System.Data.CommandType.StoredProcedure);
                return HandlePagingModel(result);
            }
        }

        /// <summary>
        /// Hàm thực hiện xử lí dữ liệu paging trả về
        /// </summary>
        /// <param name="gridReader"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual PagingModel<T> HandlePagingModel(GridReader gridReader)
        {
            return new PagingModel<T>()
            {
                Data = gridReader.Read<T>(),
                TotalRecord = gridReader.Read<int>().FirstOrDefault(),
            };
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
        public int Update(T entity, Guid id)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.Update);
            using (var connection = GetConnection())
            {
                DynamicParameters dynamicParams = DatabaseUtility.BuildParam(procName, connection, entity);
                int rowEffects = connection.Execute(procName, dynamicParams, commandType: CommandType.StoredProcedure);
                if (rowEffects > 0)
                {
                    return rowEffects;
                }
                else
                {
                    return 0;
                }
            }
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
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.Insert);
            using (var connection = GetConnection())
            {
                DynamicParameters dynamicParams = DatabaseUtility.BuildParam(procName, connection, entity);
                int rowEffects = connection.Execute(procName, dynamicParams, commandType: CommandType.StoredProcedure);
                if (rowEffects > 0)
                {
                    return rowEffects;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion



        #region DELETE
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="guids">danh sách dữ liệu cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Delete(string[] id)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.DeleteById);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ids", string.Join(",", id));
            using (var connection = GetConnection())
            {
                // khởi tạo transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowEffects = connection.Execute(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();
                        return rowEffects;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }

                }
            }
        }
        #endregion

    }

}
