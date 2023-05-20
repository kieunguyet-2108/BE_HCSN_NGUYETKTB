using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.DepartmentBL
{
    public interface IDepartmentService : IBaseService<Department>
    {
        public string GetName(string name);
    }
}
