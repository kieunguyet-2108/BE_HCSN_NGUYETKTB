using Dapper;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.DL.Database
{
    /// <summary>
    /// Class mô tả các tiện ích giúp kết nối với database thuận lợi hơn
    /// </summary>
    /// Created By: NguyetKTB (20/05/2023)
    public class DatabaseUtility
    {

        /// <summary>
        /// Thực hiện lấy ra tên procedure lưu trữ trong database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procdureName">action mà procedure đó sẽ thực hiện</param>
        /// <returns>tên của procedure</returns>
        /// Created By: NguyetKTB (20/05/2023)
        public static string GetProcdureName<T>(MSProcdureName procdureName) where T : class
        {
            // lấy ra tên bảng 
            var tableName = typeof(T).Name;

            // lấy ra action của procdure
            var procAction = procdureName.ToString();

            // trả về tên procedure
            return string.Format(ResourceVN.Procedure_Name, tableName, procAction);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storeName"></param>
        /// <param name="includeReturnValueParameter"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (20/05/2023)
        public static MySqlParameter[] DeriveParameter(IDbConnection cnn, string storeName, bool includeReturnValueParameter)
        {
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = storeName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cnn.State != ConnectionState.Open)
                {
                    cnn.Open();
                }
                MySqlCommandBuilder.DeriveParameters((MySqlCommand)cmd);
                //cnn.Close();
                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }
                var discoveredParameters = new MySqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(discoveredParameters, 0);
                foreach (var discoveredParameter in discoveredParameters)
                {
                    discoveredParameter.Value = DBNull.Value;
                }
                return discoveredParameters;
            }
        }

        /// <summary>
        /// Build lại đống param cần thiết từ đống param truyền vào
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="cnn"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// Created By: NguyetKTB (20/05/2023)
        public static DynamicParameters BuildParam(string storeName, IDbConnection cnn, object entity)
        {
            var param = new DynamicParameters();
            var entityType = entity.GetType();
            var paramArr = DeriveParameter(cnn, storeName, true);
            foreach (var paramInfo in paramArr)
            {
                if (paramInfo != null)
                {
                    var paramName = paramInfo.ParameterName;
                    try
                    {
                        paramName = paramName.Replace(paramName.Contains("$") ? "@$" : "@", "");
                        var paramValue = entityType.GetProperty(paramName).GetValue(entity, null);
                        param.Add(paramInfo.ParameterName, paramValue, direction: paramInfo.Direction);
                    }
                    catch (Exception)
                    {

                        throw new Exception("ERROR: " + paramName.ToString() + " not found");
                    }
                }
            }
            return param;
        }


    }
}
