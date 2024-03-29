﻿using Dapper;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.DL.VoucherRepository
{
    public class VoucherRepository : BaseRepository<Voucher>, IVoucherRepository
    {


        public VoucherRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Thực hiện lấy ra dữ liệu trả về thông qua paging và filter
        /// </summary>
        /// <param name="gridReader">grid reader</param>
        /// <returns>
        /// model bao gồm data(dữ liệu trả về), tổng số bản ghi và summary của table
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public override PagingModel<Voucher> HandlePagingModel(GridReader gridReader)
        {
            var data = gridReader.Read<Voucher>();
            var summary = gridReader.Read<VoucherSummary>().FirstOrDefault();
            return new PagingModel<Voucher>()
            {
                Data = data,
                Summary = summary,
                TotalRecord = ((VoucherSummary)summary).TotalRecord,
            };
        }

    }
}
