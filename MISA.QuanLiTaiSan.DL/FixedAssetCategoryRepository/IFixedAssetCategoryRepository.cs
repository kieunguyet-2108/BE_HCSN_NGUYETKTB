using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.FixedAssetCategoryDL
{
    public interface IFixedAssetCategoryRepository : IBaseRepository<FixedAssetCategory>
    {
        /// <summary>
        /// Thực hiện lấy ra thông tin entity theo điều kiện truyền vào
        /// </summary>
        /// <param name="condition">điều kiện</param>
        /// <returns>thông tin loại tài sản theo điều kiện</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public FixedAssetCategory GetByCondition(string condition);
    }
}
