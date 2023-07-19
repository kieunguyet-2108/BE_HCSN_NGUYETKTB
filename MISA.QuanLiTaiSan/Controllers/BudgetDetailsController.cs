using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.BudgetDetailService;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Resources;

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class BudgetDetailsController : BaseController<BudgetDetail>
    {
        private IBudgetDetailService _budgetDetailService;
        public BudgetDetailsController(IBudgetDetailService budgetDetailService) : base(budgetDetailService)
        {
            _budgetDetailService = budgetDetailService;
        }

        /// <summary>
        /// Lấy ra danh sách nguồn chi phí theo tài sản và chứng từ
        /// </summary>
        /// <param name="assetId">id tài sản</param>
        /// <param name="voucherId">id chứng từ</param>
        /// <returns>
        /// 200 - lấy thành công
        /// 204 - không có dữ liệu
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (05/07/2023)
        [HttpGet("GetByAsset")]
        public IActionResult GetByAsset(string assetId, string voucherId)
        {
            IEnumerable<BudgetDetail> list = _budgetDetailService.GetByAsset(assetId, voucherId);
            if (list != null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, list);

            }
            return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, list);
        }
    }
}
