using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.Enumeration;
using System.IO;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using MISA.QuanLiTaiSan.DL.TableImportRepository;
using OfficeOpenXml;
using System.Data;
using System.Reflection;
using MISA.QuanLiTaiSan.Common.Utilities;
using System.Globalization;
using System.Collections;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;
using MISA.QuanLiTaiSan.DL.FixedAssetCategoryDL;
using MISA.QuanLiTaiSan.Common.DTO;
using static Dapper.SqlMapper;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.UnitOfWork;

namespace MISA.QuanLiTaiSan.BL.FixedAssetBL
{
    public class FixedAssetService : BaseService<FixedAsset>, IFixedAssetService
    {
        private IFixedAssetRepository _fixedAssetRepository;
        private IDepartmentRepository _departmentRepository;
        private IFixedAssetCategoryRepository _categoryRepository;
        private IFixedAssetImportRepository _fixedAssetImportRepository; // model import excel của tài sản

        #region CONSTRUCTOR

        public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository categoryRepository, IFixedAssetImportRepository fixedAssetImportRepository, IUnitOfWork unitOfWork) : base(fixedAssetRepository, unitOfWork)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _categoryRepository = categoryRepository;
            _fixedAssetImportRepository = fixedAssetImportRepository;
        }
        #endregion



        #region IMPORT EXCEL
        /// <summary>
        /// Thực hiện xử lí validate và nghiệp vụ cần thiết
        /// </summary>
        /// <param name="memoryStream">
        /// File excel
        /// </param>
        /// <returns>
        /// Danh sách tài sản và danh sách lỗi
        ///</returns>
        /// Created By: NguyetKTB (02/06/2023)
        public (List<FixedAsset>, List<ImportResponse>) ImportFixedAsset(MemoryStream memoryStream)
        {
            // khai báo mảng chứa thông tin lỗi
            ResponseModel responseModel = new ResponseModel();
            // Danh sách lỗi trả về cho người dùng (chứa số hàng lỗi , dữ liệu hàng lỗi , messages lỗi )
            List<ImportResponse> importResponses = new List<ImportResponse>();
            // Danh sách các tài sản hợp lệ 
            List<FixedAsset> fixedAssetList = new List<FixedAsset>();
            // Danh sách các field excel 
            var fieldImport = _fixedAssetImportRepository.GetList();
            if (memoryStream != null)
            {
                using (var package = new ExcelPackage(memoryStream))
                {
                    // lấy ra worksheet đầu tiên trong file
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    // đọc dữ liệu từ worksheet và thêm vào datatable
                    DataTable table = new DataTable();
                    // đọc theo cột
                    for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                    {
                        table.Columns.Add(i.ToString());
                    }
                    // đọc theo hàng
                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DataRow dataRow = table.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Value;
                        }
                        table.Rows.Add(dataRow);
                    }
                    var rows = table.Rows; // số hàng trong table
                    var columns = table.Columns; // số cột
                    var firstRow = rows[0]; // lấy ra hàng đầu tiên
                    // duyệt các hàng bắt đầu từ dòng 2
                    for (int i = 1; i < rows.Count; i++)
                    {
                        var fixedAsset = new FixedAsset();
                        var row = rows[i];
                        // chứa thông tin lỗi của 1 dòng bao gồm : tên trường lỗi + danh sách lỗi của trường đó
                        IDictionary<string, List<string>> errorImport = new Dictionary<string, List<string>>();
                        // B1: Thực hiện set lại các value cho tài sản
                        // duyệt các cột của dòng i (i thay đổi)
                        for (int j = 0; j < columns.Count; j++)
                        {
                            List<string> messages = new List<string>();// danh sách thông báo lỗi của từng cột
                            string fieldKey = "";
                            var column = columns[j];
                            var columnName = firstRow[column].ToString(); // lấy ra tên cột
                            var value = row[column].ToString(); // lấy ra giá trị của cột đó
                            // duyệt danh sách cac field được khai báo trong file import
                            foreach (var field in fieldImport)
                            {
                                // kiểm tra nếu field name = tên cột
                                if (field.field_name.Equals(columnName))
                                {
                                    // lấy ra field key (field key chính là property của tài sản)
                                    fieldKey = field.field_key;
                                    // gọi hàm xử lí data để thực hiện set value của field key (entity + prop + value)
                                    HandleDataImport(fixedAsset, fixedAsset.GetType().GetProperty(field.field_key), field.field_name, value, messages);
                                    // tạo mới id
                                    Guid guid = Guid.NewGuid();
                                    // lấy ra primary key của fixed asset
                                    var primaryKey = AttributeUtility.GetPrimaryKeyName<FixedAsset>();
                                    // set giá trị cho primary key
                                    fixedAsset.GetType().GetProperty(primaryKey).SetValue(fixedAsset, guid);
                                }
                            }
                            // kiểm tra danh sách lỗi của cột
                            if (messages.Count > 0)
                            {
                                errorImport.Add(fieldKey, messages);
                            }
                        }
                        // B2: Thực hiện validate tài sản vừa duyệt
                        ValidateData(fixedAsset);
                        ValidateService(fixedAsset, (int)MSMode.Import);
                        //thực hiện gộp phần lỗi ở Bước 1 và Bước 2
                        foreach (var item in errorImport)
                        {
                            string key = item.Key;
                            List<string> values = item.Value;

                            if (errors.ContainsKey(key))
                            {
                                errors[key].AddRange(values);
                            }
                            else
                            {
                                errors[key] = values;
                            }
                        }
                        //kiểm tra lỗi của tài sản
                        if (errors.Count > 0)
                        {
                            // nếu có lỗi thì thêm vào object import response
                            importResponses.Add(new ImportResponse(i, fixedAsset, errors));
                            errors = new Dictionary<string, List<string>>();
                        }
                        else
                        {
                            // nếu không thì thêm vào danh sách tài sản
                            fixedAssetList.Add(fixedAsset);
                        }
                    }
                }
            }
            return (fixedAssetList, importResponses);
        }


        /// <summary>
        /// Thực hiện xử lí dữ liệu trong file import
        /// </summary>
        /// <param name="entity">thông tin tài sản</param>
        /// <param name="property">property của tài sản</param>
        /// <param name="field_name">tên của trường dữ liệu</param>
        /// <param name="value">giá trị trường dữ liệu</param>
        /// <param name="messages">danh sách lỗi</param>
        /// <exception cref="MISAException"></exception>
        /// Created By: NguyetKTB (02/06/2023)
        public void HandleDataImport(FixedAsset entity, PropertyInfo? property, string field_name, object? value, List<string> messages)
        {
            // lấy ra type của propery 
            var propType = property.PropertyType;
            if (value != null)
            {
                if (propType == typeof(Guid))
                {
                    Guid id = new Guid((string)value);
                    property.SetValue(entity, id);

                }
                else if (propType == typeof(string))
                {
                    property.SetValue(entity, (string)value);
                }
                else if (propType == typeof(int))
                {
                    int intValue = Convert.ToInt32(value);
                    property.SetValue(entity, intValue);
                }
                else if (propType == typeof(decimal))
                {
                    decimal decimalValue = Convert.ToDecimal(value);
                    property.SetValue(entity, decimalValue);
                }
                else if (propType == typeof(DateTime?) || propType == typeof(DateTime))
                {
                    DateTime dateTimeValue = DateTime.Parse(value.ToString(), CultureInfo.CreateSpecificCulture("fr-FR"));
                    property.SetValue(entity, dateTimeValue.ToLocalTime());
                }
            }
        }


        #endregion


        #region VALIDATE CUSTOM FOR FIXED ASSET
        /// <summary>
        /// Thực hiện override hàm validate service để thực hiện kiểm tra cụ thể theo nghiệp vụ
        /// </summary>
        /// <param name="entity">thông tin tài sản</param>
        /// <param name="mode">trường hợp cần thực hiện kiểm tra dữ liệu</param>
        /// Created By: NguyetKTB (02/06/2023)
        protected override void ValidateService(FixedAsset entity, int mode)
        {
            // lấy ra tất cả property của entity
            var properties = entity.GetType().GetProperties();
            // duyệt qua từng property
            foreach (var property in properties)
            {
                // khởi tạo danh sách lỗi
                List<string> errorMessage = new List<string>();
                // lấy ra property name
                var propertyName = property.Name;
                // lấy ra property value
                var propertyValue = property.GetValue(entity, null);
                // nếu proValue != null thì thực hiện kiểm tra
                if (propertyValue != null)
                {
                    switch (propertyName)
                    {
                        case "fixed_asset_code":
                            break;
                        case "depreciation_year":
                            // 1. nguyên giá X tỉ lệ hao mòn năm = giá trị hao mòn năm
                            if (entity.depreciation_rate * entity.cost != entity.depreciation_year)
                            {
                                errorMessage.Add(ResourceVN.Validate_Valid_DepreciationValue);
                                errors.Add(propertyName, errorMessage);
                            }
                            break;
                        case "depreciation_rate":
                            decimal a = 1 / (decimal)entity.life_time;
                            // 1 chia số năm SD = tỉ lệ HM
                            if (a != entity.depreciation_rate)
                            {
                                errorMessage.Add(ResourceVN.Validate_Valid_DepreciationRate);
                                errors.Add(propertyName, errorMessage);
                            }
                            break;
                        case "department_id":
                            // Ngoại trừ import thì không kiểm tra department id
                            if (mode != (int)MSMode.Import)
                            {
                                Department department = _departmentRepository.GetEntityById(entity.department_id);
                                if (department == null)
                                {
                                    errorMessage.Add(string.Format(ResourceVN.Validate_NotExist, "Phòng ban"));
                                    errors.Add(propertyName, errorMessage);
                                }
                                else
                                {
                                    entity.department_code = department.department_code;
                                    entity.department_name = department.department_name;
                                }
                            }
                            break;
                        case "fixed_asset_category_id":
                            // Ngoại trừ import thì không kiểm tra category id
                            if (mode != (int)MSMode.Import)
                            {
                                FixedAssetCategory fixedAssetCategory = _categoryRepository.GetEntityById(entity.fixed_asset_category_id);
                                if (fixedAssetCategory == null)
                                {
                                    errorMessage.Add(string.Format(ResourceVN.Validate_NotExist, "Loại tài sản"));
                                    errors.Add(propertyName, errorMessage);
                                }
                                else
                                {
                                    entity.fixed_asset_category_code = fixedAssetCategory.fixed_asset_category_code;
                                    entity.fixed_asset_category_name = fixedAssetCategory.fixed_asset_category_name;
                                }
                            }
                            break;
                    }
                }
            }

            // Trường hợp thực hiện kiểm tra riêng khi import
            if (mode == (int)MSMode.Import)
            {
                // kiểm tra bắt buộc nhập department_code và department_name
                string conditionDepartment = "";
                if (entity.department_code != null)
                {
                    conditionDepartment = $" department_code = '{entity.department_code}'";
                }
                Department department = _departmentRepository.GetByCondition(conditionDepartment);
                if (department == null)
                {
                    errors.Add(nameof(entity.department_code), new List<string>() { string.Format(ResourceVN.Validate_NotExist, "Phòng ban") });
                }
                else
                {
                    // set lại department_id cho entity
                    entity.department_id = department.department_id;
                    entity.department_code = department.department_code;
                    entity.department_name = department.department_name;
                }



                // kiểm tra bắt buộc nhập fixed_asset_category_code và fixed_asset_category_name
                string conditionCategory = "";
                if (entity.fixed_asset_category_code != null)
                {
                    conditionCategory = $" fixed_asset_category_code = '{entity.fixed_asset_category_code}'";
                }
                FixedAssetCategory fixedAssetCategory = _categoryRepository.GetByCondition(conditionCategory);
                if (fixedAssetCategory == null)
                {
                    errors.Add(nameof(entity.fixed_asset_category_code), new List<string>() { string.Format(ResourceVN.Validate_NotExist, "Loại tài sản") });
                }
                else
                {
                    // set lại fixed_asset_category_id cho entity
                    entity.fixed_asset_category_id = fixedAssetCategory.fixed_asset_category_id;
                    entity.fixed_asset_category_code = fixedAssetCategory.fixed_asset_category_code;
                    entity.fixed_asset_category_name = fixedAssetCategory.fixed_asset_category_name;
                }
            }
        }

        #endregion


        /// <summary>
        /// Thực hiện insert nhiều bản ghi
        /// </summary>
        /// <param name="fixedAssetList">danh sách thông tin tài sản cần thêm mới</param>
        /// <returns>số bản ghi được thêm mới</returns>
        /// Created By: NguyetKTB (02/06/2023)
        public int InsertMultiple(List<FixedAsset> fixedAssetList)
        {
            foreach (FixedAsset fixedAsset in fixedAssetList)
            {
                var newId = Guid.NewGuid();
                fixedAsset.fixed_asset_id = newId;
                ValidateService(fixedAsset, (int)MSMode.Add);
                ValidateData(fixedAsset);
            }
            if (errors.Count > 0)
            {
                int a = 10;
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                _unitOfWork.GetTransaction();
                try
                {
                    int rowEffect = _fixedAssetRepository.InsertMultiple(fixedAssetList);
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

        /// <summary>
        /// Lấy ra tài sản theo chứng từ và kết hợp phân trang, lọc
        /// </summary>
        /// <param name="filter">điều kiện lọc</param>
        /// <returns>
        /// Danh sách tài sản theo chứng từ, phân trang, lọc
        ///</returns>
        /// Created By: NguyetKTB (20/06/2023)
        public PagingModel<FixedAsset> GetByVoucher(FilterParam filter)
        {

            string whereCondition = HandleCondition(filter);
            _unitOfWork.GetTransaction();
            try
            {
                PagingModel<FixedAsset> pagingModel = _fixedAssetRepository.GetByVoucher(filter, whereCondition);
                _unitOfWork.Commit();
                return pagingModel;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Lấy ra danh sách tài sản theo chứng từ
        /// </summary>
        /// <param name="voucherId">id chứng từ</param>
        /// <returns>Danh sách tài sản thuộc chứng từ</returns>
        /// Created By: NguyetKTB (20/06/2023)
        public IEnumerable<FixedAsset> GetListInVoucher(string voucherId)
        {
            _unitOfWork.GetTransaction();
            try
            {
                IEnumerable<FixedAsset> list = _fixedAssetRepository.GetFixedAssetsInVoucher(voucherId);
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
        /// Thực hiện kiểm tra id có liên quan tới các chứng từ khác hay không
        /// </summary>
        /// <param name="guidIds">mảng voucher id</param>
        /// <returns>
        /// Số lượng tài sản có liên quan tới các chứng từ khác
        ///</returns>
        /// Created By: NguyetKTB (20/06/2023)
        public int FindAssetInVoucher(string[] guidIds)
        {
            _unitOfWork.GetTransaction();
            try
            {
                IEnumerable<FixedAsset> fixedAssets = _fixedAssetRepository.FindAssetInVoucher(guidIds);
                _unitOfWork.Commit();
                return fixedAssets.Count();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }


    }
}
