using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.VoucherDetailRepository
{
    public interface IVoucherDetailRepository : IBaseRepository<VoucherDetail>
    {
        /// <summary>
        /// Thực hiện insert nhiều tài sản
        /// </summary>
        /// <param name="list">danh sách chi tiết chứng từ cần thêm mới</param>
        /// <returns>số bản ghi được thêm mới</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public int InsertMultiple(List<VoucherDetail> list);

        /// <summary>
        /// Thực hiện xóa nhiều chi tiết chứng từ theo trường được truyền vào
        /// </summary>
        /// <param name="ids">danh sách id chi tiết chứng từ</param>
        /// <param name="field">trường muốn xóa theo</param>
        /// <returns>số bản ghi bị xóa</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int DeleteByField(string[] ids, string field);
    }
}
