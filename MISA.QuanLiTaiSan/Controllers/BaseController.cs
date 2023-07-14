using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Pagination;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.Entities;
using Microsoft.AspNetCore.Cors;
using MISA.QuanLiTaiSan.Common.Enumeration;
using System.Collections.Generic;
using static Dapper.SqlMapper;
using MISA.QuanLiTaiSan.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using MISA.QuanLiTaiSan.Common.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : class
    {

        #region INTERFACE
        private readonly IBaseService<T> _baseService;
        #endregion

        #region CONSTRUCTOR
        public BaseController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region GET
        /// <summary>
        /// Lấy ra danh sách dữ liệu
        /// </summary>
        /// <returns> 
        /// 200 - lấy danh sách dữ liệu thành công
        /// 204 - không có dữ liệu
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<T> list = _baseService.GetList();
            if (list != null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, list);

            }
            return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, list);
        }

        /// <summary>
        /// Lấy ra thông tin dữ liệu theo id truyền vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 - lấy dữ liệu thành công
        /// 204 - không có dữ liệu 
        /// 500 - lỗi 
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            T entity = _baseService.GetEntityById(id);
            if (entity == null)
            {
                return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, entity);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, entity);
            }

        }

        /// <summary>
        /// Lấy ra danh sách bản ghi có phân trang theo điều kiện
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// 200 - danh sách dữ liệu
        /// 204 - không có dữ liệu
        /// 500 - lỗi 
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpPost("GetByPaging")]
        public IActionResult GetByPaging(FilterParam filter)
        {

            PagingModel<T> pagingModel = _baseService.GetByPaging(filter);
            if (pagingModel.TotalRecord > 0)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, pagingModel);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, pagingModel);
            }
        }

        /// <summary>
        /// Thực hiện kiểm tra mã code có bị trùng lặp hay không
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpGet("DuplicateCode")]
        public IActionResult DuplicateCode(string code, string? entityId)
        {
            var entityDuplicate = _baseService.DuplicateCode(code, entityId);
            if (entityDuplicate == null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, entityDuplicate);
            }
            else
            {
                throw new MISAException(string.Format(ResourceVN.Validate_Duplicate_Code, "Mã thông tin"));
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
            string newCode = _baseService.GetNewCode();
            if (newCode != null)
            {
                return HandleResult(ResourceVN.Msg_Get_Success, MSCODE.Success, newCode);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Empty_Data, MSCODE.NoContent, newCode);
            }
        }
        #endregion
        #region INSERT
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu thêm mới</param>
        /// <returns>
        /// 201 - Thêm mới thành công
        /// 400 - Lỗi validate (dữ liệu đầu vào không hợp lệ)
        /// 500 - lỗi 
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpPost]
        public IActionResult Insert(T entity)
        {

            var guidId = _baseService.Insert(entity);
            if (guidId == Guid.Empty)
            {
                return HandleResult(ResourceVN.Msg_Failed_Insert, MSCODE.BadRequest);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Success_Insert, MSCODE.Created, guidId);
            }
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu cập nhật</param>
        /// <returns>
        /// 200 - cập nhật thành công
        /// 400 - Lỗi validate (dữ liệu đầu vào không hợp lệ)
        /// 500 - lỗi
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpPut("{entityId}")]
        public IActionResult Update([FromRoute] Guid entityId, [FromBody] T entity)
        {
            var rowAffects = _baseService.Update(entity, entityId);
            if (rowAffects > 0)
            {
                return HandleResult(ResourceVN.Msg_Success_Update, MSCODE.Success);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Failed_Update, MSCODE.BadRequest);
            }
        }

        #endregion

        #region DELETE
        /// <summary>
        /// Hàm thực hiện xử lí xóa nhiều entity
        /// </summary>
        /// <param name="ids">danh sách id cần xóa</param>
        /// <returns>
        /// 200 - xóa thành công
        /// 500 - xóa thất bại
        ///</returns>
        [HttpDelete]
        public IActionResult DeleteById(string[] ids)
        {
            int rowEffects = _baseService.Delete(ids);
            if (rowEffects > 0)
            {
                return HandleResult(ResourceVN.Msg_Success_Delete, MSCODE.Success, rowEffects);
            }
            else
            {
                return HandleResult(ResourceVN.Msg_Failed_Delete, MSCODE.BadRequest, rowEffects);
            }

        }

        #endregion


        #region HANDLE RESULT
        /// <summary>
        /// Thực hiện xử lí trả về kết quả client
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns>
        /// status code - trạng thái action
        /// response model - thông tin trả về 
        /// </returns>
        /// Created By: NguyetKTB (20/05/2023)
        protected IActionResult HandleResult(string message, MSCODE statusCode, object? data = null)
        {
            var responseModel = new ResponseModel();
            responseModel.DevMsg = message;
            responseModel.UserMsg = message;
            responseModel.Data = data;
            return StatusCode((int)statusCode, responseModel);
        }
        #endregion


    }
}
