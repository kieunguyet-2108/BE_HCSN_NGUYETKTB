using Dapper;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.DL.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.DepartmentDL
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public string GetName(string name)
        {
            return name;
        }
    }
}
