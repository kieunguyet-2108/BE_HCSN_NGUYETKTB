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
        public int DeleteMultiple(string[] vourcherIds, string field);

        public int DeleteByAsset(string[] assetIds);

        public int InsertMultiple(List<VoucherDetail> list);
    }
}
