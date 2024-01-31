using Microsoft.AspNetCore.Mvc;
using CarWash.Api.Controllers.BaseController;
using CarWash.Entity.Dtos.Auth;
using CarWash.Service.Services.Auth;
using CarWash.Entity.Dtos.Customer;
using CarWash.Entity.Dtos.Employee;
using Microsoft.AspNetCore.Authorization;
using CarWash.Entity.Enums;
namespace CarWash.Api.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]

    public class AuthController : CustomControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("custlogin")]
        public async Task<IActionResult> CustLogin([FromBody] LoginCustomerReqDto request)
        {
            var response = await _authService.CustomerLogin(request);
            return CreateActionResultInstance(response);
        }

        [HttpPost("emplogin")]
        public async Task<IActionResult> EmpLogin([FromBody] LoginEmployeeReqDto request)
        {
            var response = await _authService.EmployeeLogin(request);
            return CreateActionResultInstance(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _authService.Logout(int.Parse(GetUserId()));
            return CreateActionResultInstance(response);
        }
        /// <summary>
        /// Login
        /// </summary>
        ///<remarks>
        ///Example Request:
        ///
        ///  {
        ///     "userName": "superadmin",
        ///     "password": "123456"
        ///  }
        ///     
        /// </remarks>
        /// <returns>
        /// </returns>
        [HttpPost("custregister")]
        public async Task<IActionResult> CustRegister([FromBody] CreateCustomerDto requset)
        {
            var response = await _authService.RegisterCustomer(requset);
            return CreateActionResultInstance(response);
        }

        [HttpPost("empregister")]
        //[Authorize(Roles = $"{nameof(EmployeeRoles.Manager)}")]
        public async Task<IActionResult> EmpRegister([FromBody] CreateEmployeeDto requset)
        {
            var response = await _authService.RegisterEmployee(requset);
            return CreateActionResultInstance(response);
        }

    }
}
