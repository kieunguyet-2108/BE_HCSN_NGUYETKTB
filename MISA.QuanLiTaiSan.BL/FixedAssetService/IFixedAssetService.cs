using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.DTO;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.FixedAssetBL
{
    public interface IFixedAssetService : IBaseService<FixedAsset>
    {


        /// <summary>
        /// Thực hiện xử lí validate và nghiệp vụ cần thiết
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (02/06/2023)
        public (List<FixedAsset>, List<ImportResponse>) ImportFixedAsset(MemoryStream memoryStream);


        /// <summary>
        /// Thực hiện insert nhiều bản ghi
        /// </summary>
        /// <param name="fixedAssetList"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (02/06/2023)
        public int InsertMultiple(List<FixedAsset> fixedAssetList);

        /// <summary>
        /// Lấy ra tài sản theo chứng từ
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (20/06/2023)
        public PagingModel<FixedAsset> GetByVoucher(FilterParam filter);

        /// <summary>
        /// Lấy ra danh sách tài sản theo chứng từ
        /// </summary>
        /// <param name="voucherId">id chứng từ</param>
        /// <returns></returns>
        /// Created By: NguyetKTB (20/06/2023)
        public IEnumerable<FixedAsset> GetListInVoucher(string voucherId);

        /// <summary>
        /// Thực hiện kiểm tra id có liên quan tới các chứng từ khác hay không
        /// </summary>
        /// <param name="guidIds"></param>
        /// Created By: NguyetKTB (20/06/2023)
        public int FindAssetInVoucher(string[] guidIds);

       

    }
}
