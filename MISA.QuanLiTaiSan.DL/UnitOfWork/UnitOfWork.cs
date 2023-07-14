using Microsoft.Extensions.Configuration;
using MISA.QuanLiTaiSan.DL.Database;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region PROPERTY
        private string connectionString = DatabaseContext.ConnectionString;
        private DbConnection _dbConnection;
        private DbTransaction? _dbTransaction;
        #endregion

        #region CONSTRUCTOR
        public UnitOfWork()
        {

        }
        #endregion
        /// <summary>
        /// Khởi tạo kết nối đến database
        /// </summary>
        /// <returns>Đối tượng kết nối</returns>
        /// Created By: NguyetKTB (01/07/2023)
        public DbConnection GetConnection()
        {
            if (_dbConnection == null)
            {
                _dbConnection = new MySqlConnection(connectionString);
            }
            //if (_dbConnection.State == ConnectionState.Closed)
            //{
            //    _dbConnection.Open();
            //}
            return _dbConnection;
        }
        /// <summary>
        /// Khởi tạo transaction
        /// </summary>
        /// <returns>Đối tượng transaction</returns>
        /// Created By: NguyetKTB (01/07/2023)
        public DbTransaction GetTransaction()
        {
            if (_dbTransaction == null)
            {
                GetConnection().Open();
                _dbTransaction = _dbConnection.BeginTransaction();
            }
            return _dbTransaction;
        }
        /// <summary>
        /// Thực hiện commit transaction
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        public void Commit()
        {
            if (_dbTransaction == null)
                throw new InvalidOperationException("Transaction has not been initialized.");

            _dbTransaction.Commit();
            Close();
        }
        /// <summary>
        /// Thực hiện rollback transaction
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        public void Rollback()
        {
            if (_dbTransaction == null)
                throw new InvalidOperationException("Transaction has not been initialized.");

            _dbTransaction.Rollback();
            Close();
        }
        /// <summary>
        /// Hủy kết nối
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        public void Close()
        {
            _dbConnection.Close();
        }

        /// <summary>
        /// Mở kết nối tới database
        /// </summary>
        /// Created By: NguyetKTB (01/07/2023)
        public void OpenConnection()
        {
            if (_dbConnection.State == ConnectionState.Closed)
                _dbConnection.Open();
        }
    }
}
