using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class BudgetCategoriesController : BaseController<BudgetCategory>
    {
        public BudgetCategoriesController(IBaseService<BudgetCategory> baseService) : base(baseService)
        {
        }
    }
}
