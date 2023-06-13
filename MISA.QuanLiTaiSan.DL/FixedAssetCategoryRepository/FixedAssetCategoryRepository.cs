﻿using Dapper;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QuanLiTaiSan.Common.Enumeration;
using System.Data;

namespace MISA.QuanLiTaiSan.DL.FixedAssetCategoryDL
{
    public class FixedAssetCategoryRepository : BaseRepository<FixedAssetCategory>, IFixedAssetCategoryRepository
    {

        /// <summary>
        /// Thực hiện lấy ra thông tin entity theo điều kiện truyền vào
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (25/05/2023)
        public FixedAssetCategory GetByCondition(string condition)
        {
            string procName = DatabaseUtility.GetProcdureName<FixedAssetCategory>(MSProcdureName.GetByCondition);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("whereString", condition);
            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<FixedAssetCategory>(procName, dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
