using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.TableImportService
{
    public class FixedAssetImportService : BaseService<FixedAssetImport>, IFixedAssetImportService
    {
        public FixedAssetImportService(IBaseRepository<FixedAssetImport> baseRepository, IUnitOfWork unitOfWork) : base(baseRepository, unitOfWork)
        {
        }
    }
}
