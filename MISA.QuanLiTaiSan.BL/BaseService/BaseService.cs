using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.Utilities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.BL.BaseBL
{
    public class BaseService<T> : IBaseService<T> where T : class
    {

        private readonly IBaseRepository<T> _baseRepository;
        // khai báo danh sách chứa lỗi
        public IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        #region CONSTRUCTOR
        public BaseService(IBaseRepository<T> baseRepositoy)
        {
            _baseRepository = baseRepositoy;
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
            IEnumerable<T> list = _baseRepository.GetList();
            return list;
        }

        /// <summary>
        /// Lấy ra dữ liệu theo id
        /// </summary>
        /// <param name="id">Id cần lấy ra</param>
        /// <returns>Dữ liệu theo id truyền vào</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public T GetEntityById(Guid id)
        {

            T entity = _baseRepository.GetEntityById(id);
            return entity;
        }


        /// <summary>
        /// Lấy ra danh sách bản ghi có phân trang theo điều kiện
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual PagingModel<T> GetByPaging(FilterParam filter)
        {
            string where = ""; 
            int totalRecord = 0;
            if (filter.Filters != null)
            {
                //if (filter.Filters.Count > 0)
                //{
                //    where += " AND ";
                //}
                foreach (var filterColumn in filter.Filters)
                {
                    string whereCondition = "";
                    if (filterColumn.Operators != null)
                    {
                        // where CODE like '%a%' OR Name like '%a%' AND department_id = '11111' -> ERROR
                        // where (CODE like '%a%' OR Name like '%a%') AND department_id = '11111' -> SUCCESS
                        if (filterColumn.Operators == (int)MSFilterOperator.OR)
                        {
                            whereCondition += "(";
                            whereCondition += ProcessMixedFilterColumn(filterColumn);
                            whereCondition += ProcessGetFilterOperator(filterColumn.Operators);
                        }
                        else
                        {
                            whereCondition += ProcessMixedFilterColumn(filterColumn);
                            whereCondition += ProcessGetFilterOperator(filterColumn.Operators);
                        }
                    }
                    else
                    {
                        whereCondition += ProcessMixedFilterColumn(filterColumn);
                        whereCondition += ")";
                    }
                    where += whereCondition;
                }


            }
            // nếu where bao gồm AND hoặc OR dưới cuối thì loại bỏ
            if (where.EndsWith("AND ") || where.EndsWith("OR "))
            {
                where = where.Remove(where.Length - 4);
            }
            try
            {
                PagingModel<T> pagingModel = _baseRepository.GetByPaging(filter, where);
                return pagingModel;
            }
            catch (Exception ex)
            {
                throw new MISAException(ex.Message);
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
        public int Update(T entity, Guid id)
        {
            ValidateService(entity, (int)MSMode.Edit);
            ValidateData(entity);
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                return _baseRepository.Update(entity, id);
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
        public Guid Insert(T entity)
        {
            ValidateService(entity, (int)MSMode.Add);
            ValidateData(entity);
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                var newId = Guid.NewGuid();
                var primaryKey = AttributeUtility.GetPrimaryKeyName<T>();
                // set lại primary key cho entity = new id
                entity.GetType().GetProperty(primaryKey).SetValue(entity, newId);
                int rowEffect = _baseRepository.Insert(entity);
                if (rowEffect > 0)
                {
                    return newId;
                }
                else
                {
                    throw new MISAException(ResourceVN.Msg_Failed_Insert);
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
        public int Delete(string[] guids)
        {
            int row = _baseRepository.Delete(guids);
            return row;
        }
        #endregion


        #region COMMON
        /// <summary>
        /// Thực hiện trả về operator của câu lệnh where trong filter
        /// </summary>
        /// <param name="filterOperator"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual string ProcessGetFilterOperator(int? filterOperator)
        {
            switch (filterOperator)
            {
                case (int)MSFilterOperator.AND:
                    return " AND ";
                case (int)MSFilterOperator.OR:
                    return " OR ";
                default:
                    return " AND ";
            }
        }
        /// <summary>
        /// Hàm này dùng để tạo ra các câu lệnh sql gửi xuống db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="stringKey"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual string ProcessMixedFilterColumn(FilterCondition filter)
        {
            string whereCondition = "";
            if (!string.IsNullOrEmpty(filter.Field))
            {
                switch (filter.Condition)
                {
                    case (int)MSFilterCondition.Equal:
                        whereCondition += $"{filter.Field} = '{filter.Value}'";
                        break;
                    case (int)MSFilterCondition.NotEqual:
                        break;
                    case (int)MSFilterCondition.IsNull:
                        break;
                    case (int)MSFilterCondition.IsNotNull:
                        break;
                    case (int)MSFilterCondition.Like:
                        whereCondition += $"({filter.Field} LIKE '%{filter.Value}%' )";
                        break;
                    case (int)MSFilterCondition.NotLike:
                        break;
                    case (int)MSFilterCondition.StartsWith:
                        break;
                    case (int)MSFilterCondition.EndsWith:
                        break;
                }
            }
            return whereCondition;
        }
        #endregion


        #region VALIDATE

        /// <summary>
        /// Thực hiện kiểm tra dữ liệu truyền vào
        /// </summary>
        /// <param name="entity"></param>
        /// Created By: NguyetKTB (15/05/2023)
        protected virtual void ValidateData(T entity)
        {
            if (entity == null)
            {
                throw new MISAException(ResourceVN.Msg_Exception);
            }
            else
            {
                // lấy ra tất cả thuộc tính của đối tượng
                var properties = typeof(T).GetProperties();
                // duyệt từng phần tử list attribute
                foreach (var property in properties)
                {
                    // lấy ra tất cả attribute của property
                    var propertyAttributes = property.GetCustomAttributes(true);
                    // lấy ra value của property 
                    var propertyValue = property.GetValue(entity);
                    // khởi tạo danh sách lỗi cho property
                    List<string> errorMessages = new List<string>();
                    if (propertyValue != null)
                    {
                        // duyệt tất cả attribute của property
                        foreach (var attri in propertyAttributes)
                        {
                            // thực hiện kiểm tra 
                            switch (attri)
                            {
                                case RequiredAttribute requiredAttribute:
                                    if (propertyValue == "" || propertyValue == null)
                                    {
                                        if (((RequiredAttribute)attri).ErrorMessage != null)
                                        {
                                            errorMessages.Add(((RequiredAttribute)attri).ErrorMessage);
                                        }
                                        else
                                        {
                                        }
                                    }
                                    break;
                                case ValidLengthAttribute validLengthAttribute:
                                    int min = ((ValidLengthAttribute)attri).MinLength;
                                    int max = ((ValidLengthAttribute)attri).MaxLength;
                                    string errorMsg = ((ValidLengthAttribute)attri).ErrorMessage;
                                    string value = (string)propertyValue;
                                    if (value.Length < min || value.Length > max)
                                    {
                                        errorMessages.Add(errorMsg);
                                    }
                                    break;

                            }

                        }
                    }

                    if (errorMessages.Count > 0)
                    {
                        errors.Add(property.Name, errorMessages);
                    }

                }
            }
        }


        /// <summary>
        /// Thực hiện kiểm tra dữ liệu trong trường hợp theo từng nghiệp vụ của entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mode">trường hợp kiểm tra tùy theo nghiệp vụ</param>
        /// Created By: NguyetKTB (15/05/2023)
        protected virtual void ValidateService(T entity, int mode) { }

        #endregion
    }
}
