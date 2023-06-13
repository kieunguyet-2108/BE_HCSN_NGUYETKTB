using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.Common.Attributes
{
    public class MSAttribute : Attribute
    {
        /// <summary>
        /// Primary key attribute
        /// </summary>
        /// Created by: NguyetKTB (23/05/2023)
        [AttributeUsage(AttributeTargets.Property)]
        public class PrimaryKeyAttribute : Attribute
        {
        }

        /// <summary>
        /// Attribute Unique
        /// </summary>
        /// Created by: NguyetKTB (23/05/2023)
        [AttributeUsage(AttributeTargets.Property)]
        public class UniqueAttribute : Attribute
        {

            public string ErrorMessage { get; set; }

            public string ResourceName { get; set; }

            public string ResourceKey { get; set; }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="errorMessage">Thông báo lỗi</param>
            /// Created by: 
            public UniqueAttribute(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="resourceName">Tên resource</param>
            /// <param name="resourceKey">Key resource</param>
            /// Created by:
            public UniqueAttribute(string resourceName, string resourceKey)
            {
                ResourceName = resourceName;
                ResourceKey = resourceKey;
            }
        }


        /// <summary>
        /// Attribute TableName
        /// </summary>
        /// Created by: NguyetKTB (23/05/2023)
        [AttributeUsage(AttributeTargets.Class)]
        public class TableNameAttribute : Attribute
        {
            public string TableName { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="tableName">Tên bảng</param>
            /// Created by:
            public TableNameAttribute(string tableName)
            {
                TableName = tableName;
            }
        }


        /// <summary>
        /// Required attribute
        /// </summary>
        /// Created by: NguyetKTB (23/05/2023)
        [AttributeUsage(AttributeTargets.Property)]
        public class RequiredAttribute : Attribute
        {
            public string ErrorMessage { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="errorMessage">Thông báo lỗi</param>
            /// Created by: 
            public RequiredAttribute(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }
        }


        /// <summary>
        /// Attribute cho độ dài property
        /// </summary>
        /// Created by: NguyetKTB (23/05/2023)
        [AttributeUsage(AttributeTargets.Property)]
        public class ValidLengthAttribute : Attribute
        {
            public int MinLength { get; set; }
            public int MaxLength { get; set; }
            public string ErrorMessage { get; set; }

            public ValidLengthAttribute(int minLength, int maxLength, string errorMessage)
            {
                MinLength = minLength;
                MaxLength = maxLength;
                ErrorMessage = errorMessage;
            }
        }


    }
}
