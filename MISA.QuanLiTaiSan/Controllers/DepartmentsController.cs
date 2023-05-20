using Microsoft.AspNetCore.Mvc;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using MISA.QuanLiTaiSan.DL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.QuanLiTaiSan.Api.Controllers
{
    public class DepartmentsController : BaseController<Department>
    {
        private IDepartmentService _departmentService;
        public DepartmentsController(IBaseService<Department> baseService, IDepartmentService departmentService) : base(baseService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetName")]
        public IActionResult GetName(string name)
        {
            return StatusCode(200, _departmentService.GetName(name));
        }
    }
}
