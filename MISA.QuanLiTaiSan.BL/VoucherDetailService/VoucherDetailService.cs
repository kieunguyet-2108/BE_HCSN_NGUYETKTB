using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.VoucherService;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.BudgetDetailRepository;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;
using MISA.QuanLiTaiSan.DL.VoucherDetailRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.BL.VoucherDetailService
{
    public class VoucherDetailService : BaseService<VoucherDetail>, IVoucherDetailService
    {

        public IVoucherDetailRepository _voucherDetailRepository;
        public IFixedAssetRepository _fixedAssetRepository;
        public IBudgetDetailRepository _budgetDetailRepository;
        public VoucherDetailService(IVoucherDetailRepository voucherDetailRepository, IUnitOfWork unitOfWork, IFixedAssetRepository fixedAssetRepository, IBudgetDetailRepository budgetDetailRepository) : base(voucherDetailRepository, unitOfWork)
        {
            _voucherDetailRepository = voucherDetailRepository;
            _fixedAssetRepository = fixedAssetRepository;
            _budgetDetailRepository = budgetDetailRepository;
        }



        /// <summary>
        /// Xử lý hành động của tài sản thuộc chứng từ
        /// </summary>
        /// <param name="fixedAsset"></param>
        /// <param name="voucher"></param>
        /// <param name="action"></param>
        public void HandleActionOfAsset(VoucherDetail detail, Voucher voucher, int action)
        {
            switch (action)
            {
                case (int)MSAction.Add:
                    // gọi repo insert fixed asset 
                    List<VoucherDetail> voucherDetails = new List<VoucherDetail>();
                    voucherDetails.Add(detail);
                    _voucherDetailRepository.InsertMultiple(voucherDetails);
                    break;
                case (int)MSAction.Edit:
                    // gọi repo edit: fixed
                    break;
                case (int)MSAction.Delete:
                    string assetId = detail.fixed_asset_id.ToString();
                    // khai báo mảng asset id
                    string[] assetIds = new string[1];
                    assetIds[0] = assetId;
                    // xóa những nguồn chi phí liên quan tài sản
                    _budgetDetailRepository.DeleteByAssets(assetIds);
                    // xóa tài sản thuộc chứng từ (vì 1 tài sản chỉ thuộc 1 chứng từ nên chỉ cần truyền id tài sản vào là xóa được)
                    _voucherDetailRepository.DeleteByAsset(assetIds);
                    break;
            }
        }

    }
}
