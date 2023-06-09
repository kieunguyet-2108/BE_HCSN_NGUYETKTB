﻿using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.DL.BudgetCategoryRepository
{
    public class BudgetCategoryRepository : BaseRepository<BudgetCategory>, IBudgetCategoryRepository
    {
        public BudgetCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
