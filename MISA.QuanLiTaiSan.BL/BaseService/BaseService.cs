using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.Common.Utilities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.BL.BaseBL
{
    public class BaseService<T> : IBaseService<T> where T : class
    {

        private readonly IBaseRepository<T> _baseRepository;
        public readonly IUnitOfWork _unitOfWork;
        public IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();// khai báo danh sách chứa lỗi
        private IFixedAssetRepository fixedAssetRepository;
        private IBaseRepository<Department> baseRepository;
        private IBaseRepository<FixedAssetCategory> baseRepository1;

        #region CONSTRUCTOR
        public BaseService(IBaseRepository<T> baseRepository, IUnitOfWork unitOfWork)
        {
            _baseRepository = baseRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region GET 
        /// <summary>
        /// Hàm thực hiện lấy mã tài sản mới
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewCode()
        {

            _unitOfWork.GetTransaction(); // mở transaction
            try
            {
                string newCode = _baseRepository.GetNewCode();
                _unitOfWork.Commit();
                return newCode;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Lấy ra danh sách dữ liệu
        /// </summary>
        /// <returns>Danh sách dữ liệu </returns>
        /// Created By: NguyetKTB (15/05/2023)
        public IEnumerable<T> GetList()
        {

            _unitOfWork.GetTransaction(); // mở transaction
            try
            {
                IEnumerable<T> list = _baseRepository.GetList();
                _unitOfWork.Commit();
                return list;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
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
            _unitOfWork.GetTransaction(); // mở transaction
            try
            {
                T entity = _baseRepository.GetEntityById(id);
                _unitOfWork.Commit();
                return entity;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Lấy ra danh sách bản ghi có phân trang theo điều kiện
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual PagingModel<T> GetByPaging(FilterParam filter)
        {
            string whereCondition = HandleCondition(filter);
            _unitOfWork.GetTransaction(); // mở transaction
            try
            {
                PagingModel<T> pagingModel = _baseRepository.GetByPaging(filter, whereCondition);
                _unitOfWork.Commit();
                return pagingModel;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
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
            ValidateService(entity, (int)MSMode.Edit); // kiểm tra dữ liệu theo nghiệp vụ
            ValidateData(entity, true); // kiểm tra dữ liệu
            // nếu danh sách lỗi > 0 thì trả về lỗi cho người dùng
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                _unitOfWork.GetTransaction(); // mở transaction
                try
                {
                    // gọi repo thực hiện update
                    var rowEffect = _baseRepository.Update(entity, id);
                    _unitOfWork.Commit();
                    return rowEffect;
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
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
        public Guid Insert(T entity)
        {
            // kiểm tra dữ liệu
            ValidateService(entity, (int)MSMode.Add);
            ValidateData(entity);
            // trả về lỗi cho người dùng nếu có
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                var primaryKey = AttributeUtility.GetPrimaryKeyName<T>(); // lấy ra khóa chính của entity
                _unitOfWork.GetTransaction();
                try
                {
                    // tạo id mới cho entity
                    var entityId = Guid.NewGuid();
                    // gán id mới cho entity
                    var propertyId = entity.GetType().GetProperty(primaryKey);
                    propertyId.SetValue(entity, entityId);
                    int rowEffect = _baseRepository.Insert(entity);
                    // trả về entity.primaryKey
                    _unitOfWork.Commit();
                    return entityId;
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }
            }

        }

        #endregion


        #region DELETE
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="guids">danh sách id dữ liệu cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public virtual int Delete(string[] guids)
        {
            _unitOfWork.GetTransaction();
            try
            {
                int row = _baseRepository.Delete(guids);
                _unitOfWork.Commit();
                return row;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

        }
        #endregion


        #region COMMON
        /// <summary>
        /// Thực hiện trả về operator của câu lệnh where trong filter
        /// </summary>
        /// <param name="filterOperator">operator sẽ thực hiện filter</param>
        /// <returns>chuỗi chứa toán thử sẽ được thực hiện</returns>
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
        /// Hàm này dùng để xử lí dữ liệu lọc được gửi lên 
        /// </summary>
        /// <param name="filter">thông tin </param>
        /// <returns>chuỗi chứa câu lệnh được thực hiện để lọc</returns>
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
                    case (int)MSFilterCondition.In:
                        break;
                    case (int)MSFilterCondition.NotIn:
                        string tempCondition = "";
                        JsonElement jsonElement = (JsonElement)filter.Value;
                        foreach (JsonElement element in jsonElement.EnumerateArray())
                        {
                            tempCondition += $"'{element.GetString()}',";
                        }
                        tempCondition = tempCondition.Remove(tempCondition.Length - 1);
                        whereCondition += $"({filter.Field} NOT IN ({tempCondition}))";
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
        protected virtual void ValidateData(T entity, bool isUpdate = false)
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
                                case UniqueAttribute uniqueAttribute:
                                    // lấy ra primary key
                                    string primaryKey = AttributeUtility.GetPrimaryKeyName<T>();
                                    object duplicateEntity = null;
                                    if (isUpdate)
                                    {
                                        object primaryKeyValue = entity.GetType().GetProperty(primaryKey).GetValue(entity);
                                        duplicateEntity = _baseRepository.CheckDuplicate((string)propertyValue, primaryKeyValue.ToString());
                                    }
                                    else
                                    {
                                        // kiểm tra xem giá trị của property có bị trùng không
                                        duplicateEntity = _baseRepository.CheckDuplicate((string)propertyValue, "");
                                    }
                                    if (duplicateEntity != null)
                                    {
                                        errorMessages.Add(string.Format(ResourceVN.Validate_Duplicate_Code, "Mã tài sản"));
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

        protected virtual string HandleCondition(FilterParam filter)
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
            return where;
        }

        #endregion

        /// <summary>
        /// Kiểm tra mã code có trùng hay không
        /// </summary>
        /// <param name="code">entity code</param>
        /// <param name="id">entity id</param>
        /// <returns>
        /// nếu trùng sẽ trả về dữ liệu trùng
        /// nếu không thì trả về null
        /// </returns>
        ///Created By: NguyetKTB (25/05/2023)
        public T DuplicateCode(string code, string? id)
        {
            // lấy ra primary key
            T duplicateEntity = null;
            _unitOfWork.GetTransaction();
            try
            {
                if (id != null)
                {
                    duplicateEntity = _baseRepository.CheckDuplicate(code, id);
                }
                else
                {
                    // kiểm tra xem giá trị của property có bị trùng không
                    duplicateEntity = _baseRepository.CheckDuplicate(code, "");
                }
                _unitOfWork.Commit();
                return duplicateEntity;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
