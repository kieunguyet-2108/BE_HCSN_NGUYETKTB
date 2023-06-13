using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Enumeration
{
    /// <summary>
    /// Liệt kê các status code trả về
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public enum MSCODE
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 200,

        /// <summary>
        /// Thêm thành công
        /// </summary>
        Created = 201,

        /// <summary>
        /// Không có dữ liệu trả về
        /// </summary>
        NoContent = 204,

        /// <summary>
        /// Lỗi validate
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// lỗi do exception
        /// </summary>
        Error = 500

    }

    /// <summary>
    /// Liệt kê các tên procedure
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public enum MSProcdureName
    {
        /// <summary>
        /// Lấy ra tất cả dữ liệu
        /// </summary>
        GetAll,
        /// <summary>
        /// Lấy dữ liệu theo id
        /// </summary>
        GetById,
        /// <summary>
        /// Update dữ liệu
        /// </summary>
        Update,
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        DeleteById,
        /// <summary>
        /// Lấy dữ liệu thông qua paging và filter
        /// </summary>
        GetByPaging,
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        Insert,
        /// <summary>
        /// Lấy ra dữ liệu theo mã code
        /// </summary>
        GetByCode,
        /// <summary>
        /// Lấy ra thông tin của bảng , ví dụ tổng record...
        /// </summary>
        GetInformation,
        /// <summary>
        /// Kiểm tra mã code trùng khi update
        /// </summary>
        ExistUpdate,
        /// <summary>
        /// Lấy ra thông tin entity theo điều kiện truyền vào
        /// </summary>
        GetByCondition,

    }

    /// <summary>
    /// Liệt kê các operator filter
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public enum MSFilterCondition
    {
        Equal = 0,
        NotEqual = 1,
        IsNull = 2,
        IsNotNull = 3,
        Like = 4,
        NotLike = 5,
        StartsWith = 6,
        EndsWith = 7
    }

    /// <summary>
    /// Liệt kê các operator filter
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public enum MSFilterOperator
    {
        AND = 0,
        OR = 1,
        IN = 2
    }

    /// <summary>
    /// Liệt kê các mode của form
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public enum MSMode
    {
        Add = 1,
        Edit = 2,
        Duplicate = 3,
        Import = 4,
    }
    /// <summary>
    /// Liệt kê các type của property
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public enum MSFieldType
    {
        Int = 1,
        String = 2,
        Decimal = 3,
        Guid = 4,
        DateTime = 5,
    }
}
