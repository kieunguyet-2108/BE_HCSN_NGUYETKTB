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
