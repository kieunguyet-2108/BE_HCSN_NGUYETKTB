using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.DTO
{
    /// <summary>
    /// Lớp chứa thông tin trả về khi thực hiện import
    /// </summary> 
    /// Created By: NguyetKTB (05/06/2023)
    public class ImportResponse
    {

        #region Property

        /// <summary>
        /// Hàng trong file import
        /// </summary> 
        /// Created By: NguyetKTB (05/06/2023)
        public int Row { get; set; }

        /// <summary>
        /// Dữ liệu của hàng trong file import
        /// </summary> 
        /// Created By: NguyetKTB (05/06/2023)
        public object? RowData { get; set; }

        /// <summary>
        /// Danh sách thông báo trả về của hàng trong file (có thể bao gồm các thông báo lỗi)
        /// </summary> 
        /// Created By: NguyetKTB (05/06/2023)
        public IDictionary<string, List<string>>? Messages { get; set; }

        #endregion

        #region CONSTRUCTOR

        public ImportResponse() { }

        public ImportResponse(int row, object? rowData, IDictionary<string, List<string>>? data)
        {
            Row = row;
            RowData = rowData;
            Messages = data;
        }
        #endregion
    }
}
