using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.DL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.QuanLiTaiSan.Api.Controllers
{

    public class FixedAssetsController : BaseController<FixedAsset>
    {
        public FixedAssetsController(IBaseService<FixedAsset> baseService) : base(baseService)
        {
        }
    }
}
