using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.BL.VoucherService;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Resources;
using System;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class VouchersController : BaseController<Voucher>
    {
        private IVoucherService _voucherService;
        public VouchersController(IVoucherService voucherService) : base(voucherService)
        {
            _voucherService = voucherService;
        }


        /// <summary>
        /// Thực hiện thêm mới chứng từ
        /// </summary>
        /// <param name="param">thông tin chứng từ, tài sản 
        /// và nguồn chi phí liên quan tới chứng từ</param>
        /// <returns>
        /// 200 - thêm mới thành công
        /// 400 - lỗi validate
        /// 500 - lỗi serve
        /// </returns>
        /// Created By: NguyetKTB (05/07/2023)
        [HttpPost("InsertVoucher")]
        public IActionResult InsertVoucher(VoucherParam param)
        {
            var rs = _voucherService.InsertVoucher(param.Voucher, param.FixedAssets, param.BudgetDetails);
            if (rs == null)
            {
                return HandleResult(ResourceVN.Msg_Failed_Insert, MSCODE.BadRequest);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Success_Insert, MSCODE.Created, rs);
            }
        }

        /// <summary>
        /// Thực hiện cập nhật thông tin chứng từ và các thông tin liên quan như danh sách tài sản của chứng từ, nguồn chi phí
        /// </summary>
        /// <param name="param">thông tin được cập nhật</param>
        /// <returns>
        /// 200 - cập nhật thành công
        /// 400 - lỗi validate
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (10/07/2023)
        [HttpPut("UpdateVoucher")]
        public IActionResult UpdateVoucher(VoucherParam param)
        {
            int rowEfftect = _voucherService.UpdateVoucher(param.Voucher, param.FixedAssets, param.BudgetDetails);
            if (rowEfftect > 0)
            {
                return HandleResult(ResourceVN.Msg_Success_Update, MSCODE.Success, rowEfftect);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Failed_Update, MSCODE.BadRequest);
            }
        }

    }
}
