using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Entities
{

    /// <summary>
    /// Lớp chứa thông tin các trường trong file import
    /// </summary> 
    /// Created By: NguyetKTB (05/06/2023)
    [TableName("import_file")]
    public class FixedAssetImport
    {
        #region Property

        /// <summary>
        /// Id của cột dữ liệu
        /// </summary>
        /// Created By: NguyetKTB (02/06/2023)
        public Guid field_import_id { get; set; }

        /// <summary>
        /// Tên cột dữ liệu
        /// </summary>
        /// Created By: NguyetKTB (02/06/2023)
        public string field_name { get; set; }

        /// <summary>
        /// Key của cột dữ liệu và sẽ map với tên trong database
        /// </summary>
        /// Created By: NguyetKTB (02/06/2023)
        public string field_key { get; set; }

        /// <summary>
        /// Loại dữ liệu
        /// </summary>
        /// Created By: NguyetKTB (02/06/2023)
        public string field_type { get; set; }

        #endregion

        #region CONSTRUCTOR
        public FixedAssetImport()
        {
        }
        #endregion
    }
}
