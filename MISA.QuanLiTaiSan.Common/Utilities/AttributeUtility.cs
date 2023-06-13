using MISA.QuanLiTaiSan.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.QuanLiTaiSan.Common.Attributes.MSAttribute;

namespace MISA.QuanLiTaiSan.Common.Utilities
{
    /// <summary>
    /// Hàm này thực hiện xử lí một số tiện ích liên quan tới attribute
    /// </summary>
    /// CreatedBy: Created By: NguyetKTB (15/05/2023)  
    public class AttributeUtility
    {
        /// <summary>
        /// Lấy ra các property có attribute truyền vào
        /// </summary>
        /// <typeparam name="T">Entity truyền vào</typeparam>
        /// <param name="attributeType">Kiểu attribute cần lấy</param>
        /// <returns>Danh sách các property có attribute truyền vào</returns>
        /// CreatedBy: Created By: NguyetKTB (15/05/2023)  
        public static List<string> GetPropertiesByAttribute<T>(Type attributeType)
        {
            // lấy ra properties của T
            var properties = typeof(T).GetProperties();
            List<string> propertyNames = new List<string>();
            // duyệt danh sách properties
            foreach (var property in properties)
            {

                var attribute = (Attribute?)Attribute.GetCustomAttribute(property, attributeType);
                if (attribute != null)
                {
                    propertyNames.Add(property.Name);
                }
            }
            return propertyNames;
        }

        /// <summary>
        /// Lấy property của entity là khóa chính
        /// </summary>
        /// <typeparam name="T">Entity truyền vào</typeparam>
        /// <returns>Property là khóa chính</returns>
        /// CreatedBy:  Created By: NguyetKTB (15/05/2023)  
        public static string GetPrimaryKeyName<T>()
        {
            var result = GetPropertiesByAttribute<T>(typeof(PrimaryKeyAttribute));
            if (result.Count > 0)
            {
                return result[0].ToString();
            }
            else
            {
                throw new Exception(string.Format("Không tìm thấy attribute", typeof(T).Name));
            }
        }

        /// <summary>
        /// Lấy ra tên của bảng
        /// </summary>
        /// <typeparam name="T">Entity truyền vào</typeparam>
        /// <returns>Tên bảng</returns>
        /// CreatedBy: Created By: NguyetKTB (15/05/2023)  
        public static string GetTableName<T>(Type attributeType)
        {
            // attributeType của class
            var tableName = typeof(T).GetCustomAttributes(attributeType, true);
            if (tableName.Length > 0)
            {
                return ((TableNameAttribute)tableName[0]).TableName;
            }
            else
            {
                throw new Exception(string.Format("Không tìm thấy attibute", typeof(T).Name));
            }
        }
        
    }
}
