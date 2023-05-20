using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.DepartmentDL
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        public string GetName(string name);
    }
}
