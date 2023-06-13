using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.DTO;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.Entities;
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
        /// Hàm thực hiện lấy mã tài sản mới
        /// </summary>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewFixedAssetCode();

        /// <summary>
        /// Hàm thực hiện lấy tài sản theo mã tài sản
        /// </summary>
        /// <param name="code">mã tài sản</param>
        /// <param name="id">id tài sản</param>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public FixedAsset GetFixedAssetByCode(string code, string? id);

        /// <summary>
        /// Thực hiện xử lí validate và nghiệp vụ cần thiết
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (02/06/2023)
        public Tuple<List<FixedAsset>, List<ImportResponse>> ImportFixedAsset(MemoryStream memoryStream);


        /// <summary>
        /// Thực hiện insert nhiều bản ghi
        /// </summary>
        /// <param name="fixedAssetList"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (02/06/2023)
        public int InsertMultiple(List<FixedAsset> fixedAssetList);

    }
}
