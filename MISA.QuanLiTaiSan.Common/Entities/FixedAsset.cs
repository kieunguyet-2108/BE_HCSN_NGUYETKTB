using MISA.QuanLiTaiSan.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities

{

    /// <summary>
    /// Lớp tài sản
    /// </summary> 
    /// Created By: NguyetKTB (15/05/2023)
    [TableName("fixed_asset")]
    public class FixedAsset
    {
        #region Properties
        /// <summary>
        /// Id tài sản
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        [PrimaryKey]
        [Required("Id tài sản không được để trống.")]
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Unique("Mã tài sản đã tồn tại")]
        [Required("Mã tài sản không được để trống. ")]
        [ValidLength(1, 100, "Độ dài mã tài sản không hợp lệ.")]
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Tên tài sản không được để trống.")]
        [ValidLength(1, 255, "Độ dài tên tài sản không hợp lệ.")]
        public string fixed_asset_name { get; set; }
        /// <summary>
        /// Id phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Id phòng ban không được để trống. ")]
        public Guid department_id { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [ValidLength(0, 50, "Độ dài mã phòng ban không hợp lệ.")]
        public string? department_code { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [ValidLength(0, 255, "Độ dài tên phòng ban không hợp lệ.")]
        public string? department_name { get; set; }

        /// <summary>
        /// Id loại tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Id mã loại tài sản không được để trống. ")]
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        [ValidLength(0, 50, "Độ dài mã loại tài sản không hợp lệ.")]
        public string? fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [ValidLength(0, 255, "Độ dài tên loại tài sản không hợp lệ.")]
        public string? fixed_asset_category_name { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Nguyên giá không được để trống. ")]
        public decimal cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Số lượng không được để trống. ")]
        public int quantity { get; set; }

        /// <summary>
        /// Tỉ lệ hao mòn (%)
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Tỉ lệ hao mòn không được để trống. ")]
        public decimal depreciation_rate { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Ngày mua không được để trống. ")]
        public DateTime purchase_date { get; set; }

        /// <summary>
        /// Ngày bắt đầu sử dụng
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Ngày bắt đầu sử dụng không được để trống. ")]
        public DateTime start_using_date { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi tài sản trên phần mềm
        /// </summary> 
        /// Created By: NguyetKTB (15/05/2023)
        public int tracked_year { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Số năm sử dụng không được để trống. ")]
        public int life_time { get; set; }

        /// <summary>
        /// Năm sử dụng
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        public int production_year { get; set; }


        /// <summary>
        /// Gía trị hao mòn năm
        /// </summary>  
        /// Created By: NguyetKTB (15/05/2023)
        [Required("Giá trị hao mòn năm không được để trống. ")]
        public decimal depreciation_year { get; set; }

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
