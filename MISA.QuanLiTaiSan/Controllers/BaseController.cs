using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.Common.Exceptions;

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
        /// <returns> danh sách dữ liệu</returns>
        /// Created By: NguyetKTB (20/05/2023)
        [HttpGet]
        public IEnumerable<T> GetAll()
        {
            return _baseService.GetList();
        }

        /// <summary>
        /// Lấy ra thông tin dữ liệu theo id truyền vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public T GetById(Guid id)
        {
            return _baseService.GetEntityById(id);
        }
        #endregion



    }
}
