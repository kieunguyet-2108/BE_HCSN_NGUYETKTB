using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using MISA.QuanLiTaiSan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.DepartmentBL
{
    public class DepartmentService : BaseService<Department> , IDepartmentService
    {
        public DepartmentService(IBaseRepository<Department> baseRepository) : base(baseRepository)
        {
        }
    }
}
