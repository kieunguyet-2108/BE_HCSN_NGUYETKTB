using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using MISA.QuanLiTaiSan.Entities;
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

namespace MISA.QuanLiTaiSan.BL.FixedAssetBL
{
    public class FixedAssetService : BaseService<FixedAsset>, IFixedAssetService
    {
        private IFixedAssetRepository _fixedAssetRepository;
        private IDepartmentRepository _departmentRepository;
        private IFixedAssetCategoryRepository _categoryRepository;
        private IFixedAssetImportRepository _fixedAssetImportRepository; // model import excel của tài sản

        #region CONSTRUCTOR

        public FixedAssetService(IFixedAssetRepository fixedAssetRepository) : base(fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
        }
        public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository categoryRepository, IFixedAssetImportRepository fixedAssetImportRepository) : base(fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _categoryRepository = categoryRepository;
            _fixedAssetImportRepository = fixedAssetImportRepository;
        }
        #endregion


        #region GET
        /// <summary>
        /// Hàm thực hiện lấy mã tài sản mới
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewFixedAssetCode()
        {
            string newCode = _fixedAssetRepository.GetNewCode();
            return newCode;
        }

        /// <summary>
        /// Hàm thực hiện lấy tài sản theo mã tài sản
        /// </summary>
        /// <param name="code">mã tài sản</param>
        /// <param name="id">id tài sản</param>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public FixedAsset GetFixedAssetByCode(string code, string? id = null)
        {
            // nếu id = null thì lấy theo trường hợp insert
            if (string.IsNullOrEmpty(id))
            {
                return _fixedAssetRepository.CheckExistOnInsert("fixed_asset_code", code, "fixed_asset");
            }
            // nếu id khác null thì lấy theo trường hợp update
            else
            {
                Guid newGuid = Guid.Parse(id);
                return _fixedAssetRepository.CheckExistOnUpdate("fixed_asset_code", code, "fixed_asset", newGuid);

            }

        }

        #endregion


        #region IMPORT EXCEL
        /// <summary>
        /// Thực hiện xử lí validate và nghiệp vụ cần thiết
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (02/06/2023)
        public Tuple<List<FixedAsset>, List<ImportResponse>> ImportFixedAsset(MemoryStream memoryStream)
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
            return Tuple.Create(fixedAssetList, importResponses);
        }


        /// <summary>
        /// Thực hiện xử lí dữ liệu trong file import
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <exception cref="MISAException"></exception>
        /// Created By: NguyetKTB (02/06/2023)
        public void HandleDataImport(FixedAsset entity, PropertyInfo? property, string field_name, object? value, List<string> messages)
        {
            // lấy ra type của propery 
            var propType = property.PropertyType;
            if (value != null)
            {
                try
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
                catch (Exception ex)
                {
                    messages.Add(string.Format(ResourceVN.Validate_Valid_Format, field_name));
                }

            }
            else
            {
                throw new MISAException(ResourceVN.Msg_Exception);
            }
        }


        #endregion


        #region VALIDATE CUSTOM FOR FIXED ASSET
        /// <summary>
        /// Thực hiện override hàm validate service để thực hiện kiểm tra cụ thể theo nghiệp vụ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errors"></param>
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
                            FixedAsset duplicateEntity = new FixedAsset();
                            // 1. trường hợp insert hoặc import
                            if (mode == (int)MSMode.Add || mode == (int)MSMode.Import)
                            {
                                duplicateEntity = _fixedAssetRepository.CheckExistOnInsert(propertyName, propertyValue, "fixed_asset");
                            }
                            // 2. trường hợp update
                            else if (mode == (int)MSMode.Edit)
                            {
                                duplicateEntity = _fixedAssetRepository.CheckExistOnUpdate(propertyName, propertyValue, "fixed_asset", entity.fixed_asset_id);
                            }
                            // kiểm tra duplicate hay không
                            if (duplicateEntity != null)
                            {
                                errorMessage.Add(string.Format(ResourceVN.Validate_Duplicate_Code, "Mã tài sản"));
                                errors.Add(propertyName, errorMessage);
                            }
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
                            try
                            {
                                decimal a = 1 / (decimal)entity.life_time;
                                // 1 chia số năm SD = tỉ lệ HM
                                if (a != entity.depreciation_rate)
                                {
                                    errorMessage.Add(ResourceVN.Validate_Valid_DepreciationRate);
                                    errors.Add(propertyName, errorMessage);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new MISAException(ex.Message);
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
                //if (entity.department_code != null && entity.department_name != null)
                //{
                //    conditionDepartment = $" department_code = '{entity.department_code}' AND department_name = '{entity.department_name}'";

                //}
                //else if (entity.department_name != null && entity.department_code == null)
                //{
                //    conditionDepartment = $" department_name = '{entity.department_name}'";
                //}
                //else if (entity.department_code != null && entity.department_name == null)
                //{
                //    conditionDepartment = $" department_code = '{entity.department_code}'";
                //}

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

                //if (entity.fixed_asset_category_code != null)
                //{
                //    conditionCategory = $" fixed_asset_category_code = '{entity.fixed_asset_category_code}'";
                //}
                //else if (entity.fixed_asset_category_name != null)
                //{
                //    conditionCategory = $" fixed_asset_category_name = '{entity.fixed_asset_category_name}'";
                //}
                //else if (entity.fixed_asset_category_code != null && entity.fixed_asset_category_name != null)
                //{
                //    conditionCategory = $" fixed_asset_category_code = '{entity.fixed_asset_category_code}' AND fixed_asset_category_name = '{entity.fixed_asset_category_name}'";
                //}

            }
        }

        /// <summary>
        /// Thực hiện override lại để thực hiện thêm các trường hợp kiểm tra dữ liệu riêng
        /// </summary>
        /// <param name="entity"></param>
        /// Created By: NguyetKTB (02/06/2023)
        protected override void ValidateData(FixedAsset entity)
        {
            base.ValidateData(entity);
        }
        #endregion


        /// <summary>
        /// Thực hiện insert nhiều bản ghi
        /// </summary>
        /// <param name="fixedAssetList"></param>
        /// <returns></returns>
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
                int rowEffect = _fixedAssetRepository.InsertMultiple(fixedAssetList);
                if (rowEffect > 0)
                {
                    return rowEffect;
                }
                else
                {
                    throw new MISAException(ResourceVN.Msg_Failed_Insert);
                }
            }

        }

    }
}
