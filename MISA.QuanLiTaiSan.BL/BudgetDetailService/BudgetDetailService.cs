using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.BudgetDetailRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.BudgetDetailService
{
    public class BudgetDetailService : BaseService<BudgetDetail>, IBudgetDetailService
    {
        private IBudgetDetailRepository _budgetDetailRepository;
        public BudgetDetailService(IBudgetDetailRepository budgetDetailRepository, IUnitOfWork unitOfWork) : base(budgetDetailRepository, unitOfWork)
        {
            _budgetDetailRepository = budgetDetailRepository;
        }

        /// <summary>
        /// Lấy ra danh sách thông tin chi tiết nguồn chi phí theo tài sản của chứng từ
        /// </summary>
        /// <param name="assetId">id của tài sản</param>
        /// <param name="voucherID">id của chứng từ</param>
        /// <returns>danh sách chi tiết chi phí</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public IEnumerable<BudgetDetail> GetByAsset(string assetId, string voucherID)
        {
            IEnumerable<BudgetDetail> budgetDetails = _budgetDetailRepository.GetByAsset(assetId, voucherID);
            return budgetDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="budgetDetail"></param>
        /// <param name="fixed_asset_id"></param>
        /// <param name="action"></param>
        public void HandleBudgetDetail(BudgetDetail budgetDetail, int action)
        {
            switch (action)
            {
                case (int)MSAction.Add:
                    // gọi repo insert fixed asset 
                    List<BudgetDetail> budgetDetailList = new List<BudgetDetail> { budgetDetail};
                    _budgetDetailRepository.InsertMultiple(budgetDetailList);
                    break;
                case (int)MSAction.Edit:
                    _budgetDetailRepository.Update(budgetDetail, budgetDetail.budget_detail_id);
                    // gọi repo edit: fixed
                    break;
                case (int)MSAction.Delete:
                    // gọi repo xóa tài sản by fixed asset id in voucher id
                    string budgetDetailId = budgetDetail.budget_detail_id.ToString();
                    // khai báo mảng asset id
                    string[] budgetIds = new string[1];
                    budgetIds[0] = budgetDetailId;
                    _budgetDetailRepository.Delete(budgetIds);
                    break;
            }
        }
    }
}
