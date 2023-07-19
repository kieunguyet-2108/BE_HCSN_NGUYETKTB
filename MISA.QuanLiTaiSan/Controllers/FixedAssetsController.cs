

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.Common.DTO;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.Common.Resources;
using System.Reflection.PortableExecutable;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class FixedAssetsController : BaseController<FixedAsset>
    {
        private IFixedAssetService _fixedAssetService;
        public FixedAssetsController(IFixedAssetService fixedAssetService) : base(fixedAssetService)
        {
            _fixedAssetService = fixedAssetService;
        }


        #region 
        /// <summary>
        /// Lấy ra tổng số chứng từ theo tài sản 
        /// nhằm thực hiện kiểm tra có chứng từ liên quan tới tài sản hay không
        /// </summary>
        /// <param name="ids">danh sách id tài sản</param>
        /// <returns>
        /// 200 - thành công, không có chứng từ liên quan
        /// 400 - có chứng từ liên quan
        /// </returns>
        /// Created By: NguyetKTB (05/07/2023)
        [HttpPost("FindInVoucher")]
        public IActionResult FindInVoucher(string[] ids)
        {
            int rs = _fixedAssetService.FindAssetInVoucher(ids);
            if (rs > 0)
            {
                return HandleResult(ResourceVN.Validate_FixedAsset_BeforeDelete, MSCODE.BadRequest, rs);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, rs);
            }
        }
        #endregion

        #region IMPORT 
        /// <summary>
        /// Thực hiện lấy ra thông tin trong file và kiểm tra dữ liệu
        /// </summary>
        /// <param name="fromFile">file</param>
        /// <returns>
        /// 200 - thành công
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (03/06/2023)
        [HttpPost("Import")]
        public IActionResult Import([FromForm] IFormFile fromFile)
        {
            IActionResult? actionResult = null;
            if (fromFile != null)
            {
                var fileName = Path.GetFileName(fromFile.FileName);
                //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // nếu filename không contains .xlsx thì return
                if (!Path.GetExtension(fromFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    actionResult = HandleResult(ResourceVN.Msg_Exception, MSCODE.BadRequest, null);
                }
                using (var stream = new MemoryStream())
                {
                    fromFile.CopyTo(stream);
                    stream.Position = 2;
                    var (fixedAssetList, importResponses) = _fixedAssetService.ImportFixedAsset(stream); // Đây là một Tuple
                    if (importResponses.Count == 0 && fixedAssetList.Count > 0)
                    {
                        actionResult = HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, fixedAssetList);
                    }
                    else
                    {
                        actionResult = HandleResult(ResourceVN.Msg_Exception, MSCODE.BadRequest, importResponses);
                    }

                }
            }
            return actionResult;
        }
        #endregion

        #region INSERT MULTIPLE
        /// <summary>
        /// Thực hiện thêm mới nhiều tài sản
        /// </summary>
        /// <param name="fixedAssets">danh sách tài sản muốn thêm mới</param>
        /// <returns>
        /// 200 - thêm mới thành côg
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (03/06/2023)
        [HttpPost("InsertMupltiple")]
        public IActionResult InsertMultiple([FromBody] List<FixedAsset> fixedAssets)
        {

            int row = _fixedAssetService.InsertMultiple(fixedAssets);
            if (row > 0)
            {
                return HandleResult(ResourceVN.Msg_Success_Insert, MSCODE.Created, row);
            }
            return HandleResult(ResourceVN.Msg_Failed_Insert, MSCODE.BadRequest, row);
        }
        #endregion


        /// <summary>
        /// Lấy ra danh sách chứng từ đã được lọc và phân trang
        /// </summary>
        /// <param name="filter">thông tin lọc và phân trang</param>
        /// <returns>
        /// 200 - thành công
        /// 204 - danh sách rỗng
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (05/07/2023)
        [HttpPost("GetByVoucher")]
        public IActionResult GetByVoucher(FilterParam filter)
        {
            PagingModel<FixedAsset> list = _fixedAssetService.GetByVoucher(filter);
            if (list != null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, list);
            }
            return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, list);
        }

        /// <summary>
        /// Lấy ra danh sách tài sản theo chứng từ
        /// </summary>
        /// <param name="voucherId">id chứng từ</param>
        /// <returns>
        /// 200 - lấy thành công
        /// 204 - không có dữ liệu
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (05/07/2023)
        [HttpGet("GetListInVoucher/{voucherId}")]
        public IActionResult GetListInVoucher(string voucherId)
        {
            IEnumerable<FixedAsset> fixedAssets = _fixedAssetService.GetListInVoucher(voucherId);
            if (fixedAssets.Count() > 0)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, fixedAssets);
            }
            return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, fixedAssets);
        }

    }


}
