using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MISAttribute;

namespace MISA.QuanLiTaiSan.DL.Entities

{

    /// <summary>
    /// Lớp tài sản
    /// </summary> 
    /// Created By: NguyetKTB (15/05/2023)
    public class FixedAsset
    {
        #region Properties
        /// <summary>
        /// Id tài sản
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        [PrimaryKey]
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string? fixed_asset_code { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string? fixed_asset_name { get; set; }
        /// <summary>
        /// Id phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public Guid? department_id { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string? department_code { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string? department_name { get; set; }

        /// <summary>
        /// Id loại tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public Guid? fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string? fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public string? fixed_asset_category_name { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public decimal? cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public int? quantity { get; set; }

        /// <summary>
        /// Tỉ lệ ho mòn (%)
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public decimal? depreciation_rate { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? purchase_date { get; set; }

        /// <summary>
        /// Ngày bắt đầu sử dụng
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? start_using_date { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi tài sản trên phần mềm
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public int? tracked_year { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public int? life_time { get; set; }

        /// <summary>
        /// Năm sử dụng
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public int? production_year { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public string? modified_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public DateTime? modified_date { get; set; }
        #endregion


    }
}
