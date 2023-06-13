

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.Common.DTO;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Entities;
using System.Reflection.PortableExecutable;

namespace MISA.QuanLiTaiSan.Api.Controllers
{

    public class FixedAssetsController : BaseController<FixedAsset>
    {
        private IFixedAssetService _fixedAssetService;
        public FixedAssetsController(IFixedAssetService fixedAssetService) : base(fixedAssetService)
        {
            _fixedAssetService = fixedAssetService;
        }

        #region GET BY CODE
        /// <summary>
        /// Lấy ra thông tin dữ liệu theo mã tài sản
        /// </summary>
        /// <param name="code">mã tài sản</param>
        /// <param name="entityId">id tài sản</param>
        /// <returns>
        /// 200 - lấy dữ liệu thành công
        /// 204 - không có dữ liệu 
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpGet("GetByCode")]
        public IActionResult GetByCode(string code, string? entityId)
        {
            var entity = _fixedAssetService.GetFixedAssetByCode(code, entityId);
            if (entity != null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, (int)MSCODE.Success, entity);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.Success, (int)MSCODE.NoContent, entity);
            }

        }
        #endregion 

        #region GET NEW CODE
        /// <summary>
        /// Thực hiện lấy ra mã tài sản mới nhất
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (28/05/2023)
        [HttpGet("GetNewCode")]
        public IActionResult GetNewCode()
        {
            string newCode = _fixedAssetService.GetNewFixedAssetCode();
            if (newCode != null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, (int)MSCODE.Success, newCode);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.Success, (int)MSCODE.NoContent, newCode);
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
            if (fromFile == null)
            {
                return HandleResult(ResourceVN.Msg_Exception, MSCODE.Success, (int)MSCODE.BadRequest, null);
            }
            else
            {
                try
                {
                    var fileName = Path.GetFileName(fromFile.FileName);
                    //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    // nếu filename không contains .xlsx thì return
                    if (!Path.GetExtension(fromFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        return HandleResult(ResourceVN.Msg_Exception, MSCODE.Success, (int)MSCODE.BadRequest, null);
                    }
                    using (var stream = new MemoryStream())
                    {
                        fromFile.CopyTo(stream);
                        stream.Position = 2;
                        Tuple<List<FixedAsset>, List<ImportResponse>> result = (Tuple<List<FixedAsset>, List<ImportResponse>>)_fixedAssetService.ImportFixedAsset(stream); // Đây là một Tuple
                        List<ImportResponse> importResponses = result.Item2;
                        List<FixedAsset> fixedAssetList = result.Item1;
                        // 
                        if (importResponses.Count == 0 && fixedAssetList.Count > 0)
                        {
                            return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, (int)MSCODE.Success, fixedAssetList, importResponses);

                        }
                        else
                        {
                            return HandleResult(ResourceVN.Msg_Exception, MSCODE.Success, (int)MSCODE.BadRequest, fixedAssetList, importResponses);
                        }

                    }
                }
                catch (MISAException ex)
                {
                    return HandleException(ex, MSCODE.Success, (int)MSCODE.BadRequest);
                }
            }
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
            if (fixedAssets.Count > 0)
            {
                try
                {
                    int row = _fixedAssetService.InsertMultiple(fixedAssets);
                    return HandleResult(ResourceVN.Msg_Success_Insert, MSCODE.Created, (int)MSCODE.Created, row);
                }
                catch (MISAException ex)
                {
                    return HandleException(ex, MSCODE.Success, (int)MSCODE.BadRequest);
                }
            }
            else
            {
                return HandleResult(ResourceVN.Msg_IsValid_InputData, MSCODE.Success, (int)MSCODE.BadRequest);
            }
        }
        #endregion

    }


}
