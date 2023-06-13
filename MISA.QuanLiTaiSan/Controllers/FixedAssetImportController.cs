using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class FixedAssetImportController : BaseController<FixedAssetImport>
    {
        public FixedAssetImportController(IBaseService<FixedAssetImport> baseService) : base(baseService)
        {
        }
    }
}
