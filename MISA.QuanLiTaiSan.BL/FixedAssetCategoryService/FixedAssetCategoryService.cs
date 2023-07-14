using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.UnitOfWork;

namespace MISA.QuanLiTaiSan.BL.FixedAssetCategoryBL
{
    public class FixedAssetCategoryService : BaseService<FixedAssetCategory>, IFixedAssetCategoryService
    {
        public FixedAssetCategoryService(IBaseRepository<FixedAssetCategory> baseRepository, IUnitOfWork unitOfWork) : base(baseRepository, unitOfWork)
        {
        }
    }
}
