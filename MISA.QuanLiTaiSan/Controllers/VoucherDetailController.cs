using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.VoucherDetailService;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Resources;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class VoucherDetailController : BaseController<VoucherDetail>
    {
        private IVoucherDetailService _voucherDetailService;
        public VoucherDetailController(IVoucherDetailService voucherDetailService) : base(voucherDetailService)
        {
            _voucherDetailService = voucherDetailService;
        }

    }
}
