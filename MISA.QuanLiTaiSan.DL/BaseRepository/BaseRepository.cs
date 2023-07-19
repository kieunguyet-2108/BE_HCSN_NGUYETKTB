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
using MISA.QuanLiTaiSan.Common.Pagination;
using System.Drawing;
using static Dapper.SqlMapper;
using System.Reflection.PortableExecutable;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using System.Data.Common;

namespace MISA.QuanLiTaiSan.DL.BaseDL
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        public readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region DATABASE CONNECTION


        /// <summary>
        /// Thực hiện kết nối với database
        /// </summary>
        /// <returns>kết nối với database</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public DbConnection GetConnection()
        {
            //// lấy ra chuỗi context kết nối tới database
            //string connectonString = DatabaseContext.ConnectionString;
            //// khởi tạo đối tượng kết nối
            //MySqlConnection connection = new MySqlConnection(connectonString);
            //// mở kết nối tới database
            //connection.Open();
            return _unitOfWork.GetConnection();
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
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            var rs =  connection.Query<T>(procdureName, commandType: CommandType.StoredProcedure, transaction:transaction);
            return rs;
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
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            return connection.QueryFirstOrDefault<T>(procedureName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        /// <summary>
        /// lấy ra dữ liệu theo paging
        /// </summary>
        /// <param name="filter">thông tin lọc, phân trang</param>
        /// <param name="where">điều kiện lọc</param>
        /// <returns>
        /// Danh sách dữ liệu theo paging, điều kiện lọc
        /// </returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual PagingModel<T> GetByPaging(FilterParam filter, string? where)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.GetByPaging);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            var result = connection.QueryMultiple(procName,
            new
            {
                ms_offset = (filter.PageNumber - 1) * filter.PageSize,
                ms_limit = filter.PageSize,
                ms_where = where
            }, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
            return HandlePagingModel(result);
        }

        /// <summary>
        /// Hàm thực hiện xử lí dữ liệu paging trả về
        /// </summary>
        /// <param name="gridReader">grid reader</param>
        /// <returns>
        /// Thông tin phân trang theo entity
        /// </returns>
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
        /// <param name="id">id của entity sẽ update</param>
        /// <returns>số bản ghi được update</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Update(T entity, Guid id)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.Update);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            DynamicParameters dynamicParams = DatabaseUtility.BuildParam(procName, connection, entity);
            var rowEffects = connection.Execute(procName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: transaction);
            return rowEffects;
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
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            DynamicParameters dynamicParams = DatabaseUtility.BuildParam(procName, connection, entity);
            var rowEffects = connection.Execute(procName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.GetTransaction());
            return rowEffects;
        }
        #endregion



        #region DELETE
        /// <summary>
        /// Xóa dữ liệu theo danh sách id
        /// </summary>
        /// <param name="id">danh sách id dữ liệu cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual int Delete(string[] id)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.DeleteById);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ids", string.Join(",", id));
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            try
            {
                var rowEffects = connection.Execute(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                return rowEffects;
            }
            catch 
            {
                throw;
            }
        }
        #endregion


        /// <summary>
        /// Hàm thực hiện lấy mã mới nhất
        /// </summary>
        /// <returns>
        /// Mã mới nhất
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewCode()
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.GetNewCode);
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            var rs = connection.QueryFirstOrDefault<string>(procName, new { }, commandType: CommandType.StoredProcedure, transaction: transaction);
            return rs;
        }

        /// <summary>
        /// Thực hiện kiểm tra mã code truyền vào có trùng lặp hay không
        /// </summary>
        /// <param name="proValue">giá trị muốn kiểm tra</param>
        /// <param name="id">id của entity</param>
        /// <returns>
        /// entity trùng lặp
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public T CheckDuplicate(string proValue, object? id)
        {
            string procName = DatabaseUtility.GetProcdureName<T>(MSProcdureName.CheckExistCode);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("code", $"'{proValue}'");
            dynamicParameters.Add("id", $"'{id}'");
            var connection = GetConnection();
            var transaction = _unitOfWork.GetTransaction();
            var rs = connection.QueryFirstOrDefault<T>(procName, dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return rs;

        }
    }

}
