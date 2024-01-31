using CarWash.Api.Controllers.BaseController;
using CarWash.Service.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : CustomControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("info")]
        public async Task<IActionResult> UserInfo()
        {
            var response = await _userService.GetUserInfo(int.Parse(GetUserId()));

            return CreateActionResultInstance(response);
        }
    }
}
