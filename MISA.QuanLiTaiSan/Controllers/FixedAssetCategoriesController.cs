using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.BL.FixedAssetCategoryBL;
using MISA.QuanLiTaiSan.DL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.QuanLiTaiSan.Api.Controllers
{

    public class FixedAssetCategoriesController : BaseController<FixedAssetCategory>
    {
        public FixedAssetCategoriesController(IBaseService<FixedAssetCategory> baseService) : base(baseService)
        {
        }
    }
}
