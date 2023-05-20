using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MISAttribute;

namespace MISA.QuanLiTaiSan.DL.Entities
{
    /// <summary>
    /// Lớp loại tài sản
    /// </summary>
    /// Created By: NguyetKTB (15/05/2023)
    public class FixedAssetCategory
    {
        #region Properties
        /// <summary>
        /// Id mã loại tài sản
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        [PrimaryKey]
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên loại tài san
        /// </summary> 
        ///  Created By: NguyetKTB (15/05/2023)
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public float depreciation_rate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public int life_time { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string description { get; set; }


        /// <summary>
        /// Người tạo
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string created_by { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        /// Created By: NguyetKTB (15/05/2023)
        public string modified_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? modified_date { get; set; }
        #endregion

    }
}
