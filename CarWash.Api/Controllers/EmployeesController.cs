using CarWash.Api.Controllers.BaseController;
using CarWash.Entity.Dtos.Auth;
using CarWash.Entity.Dtos.Employee;
using CarWash.Service.Services.EmployeeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : CustomControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("createEmpAttendance")]
        public async Task<IActionResult> CustLogin([FromBody] CreateEmployeeAttandaceDto request)
        {
            var response = await _employeeService.UpdateEmployeeAttendance(request);
            return CreateActionResultInstance(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var response = await _employeeService.GetAllEmployee();

            return CreateActionResultInstance(response);
        }
        
        [HttpGet("reports")]
        public async Task<IActionResult> GetAllEmployeeReports()
        {
            var response = await _employeeService.GetAllEmployeeRapor();

            return CreateActionResultInstance(response);
        }
        
        [HttpGet("reportDetails/{id:int}")]
        public async Task<IActionResult> GetAllEmployeeReports([FromRoute]int id)
        {
            var response = await _employeeService.GetAllEmployeeDetailRapor(id);

            return CreateActionResultInstance(response);
        }
    }
}
