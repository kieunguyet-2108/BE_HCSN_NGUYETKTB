using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.Common.Entities;
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
        ///// <summary>
        ///// Thực hiện lấy ra thông tin entity theo điều kiện truyền vào
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        ///// Created By: NguyetKTB (25/05/2023)
        public Department GetByCondition(string condition);
    }
}
