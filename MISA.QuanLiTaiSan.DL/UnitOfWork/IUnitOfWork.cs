using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.UnitOfWork
{
    public interface IUnitOfWork 
    {
        DbConnection GetConnection();
        void OpenConnection();
        DbTransaction GetTransaction();
        void Commit();
        void Rollback();
        void Close();



    }
}
