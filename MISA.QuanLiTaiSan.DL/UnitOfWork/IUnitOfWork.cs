using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Khởi tạo kết nối đến database
        /// </summary>
        /// <returns>Đối tượng kết nối</returns>
        /// Created By: NguyetKTB (01/07/2023)
        DbConnection GetConnection();
        /// <summary>
        /// Mở kết nối tới database
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        void OpenConnection();
        /// <summary>
        /// Khởi tạo transaction
        /// </summary>
        /// <returns>Đối tượng transaction</returns>
        /// Created By: NguyetKTB (01/07/2023)
        DbTransaction GetTransaction();
        /// <summary>
        /// Thực hiện commit transaction
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        void Commit();
        /// <summary>
        /// Thực hiện rollback transaction
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        void Rollback();
        /// <summary>
        /// Hủy kết nối
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        void Close();



    }
}
