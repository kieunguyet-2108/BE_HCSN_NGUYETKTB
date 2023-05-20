using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MISAttribute;

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
        /// Lấy ra property có attribute khóa chính trong bảng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>khóa chính trong bảng</returns>
        /// Created By: NguyetKTB (20/05/2023)
        public static string GetPrimaryKeyInTable<T>()
        {
            var properties = typeof(T).GetProperties();
            string primaryKey = "";
            // duyệt properties và kiểm tra property nào có attribute là primary key
            foreach (var property in properties)
            {
                var attri = (PrimaryKey)Attribute.GetCustomAttribute(property, typeof(PrimaryKey));
                if (attri != null)
                {
                    primaryKey = property.Name;
                }
            }
            return primaryKey;

        }
    }
}
