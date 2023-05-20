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
        Delete,
        /// <summary>
        /// Lấy dữ liệu thông qua paging và filter
        /// </summary>
        GetByPaging,
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        Insert

    }
}
