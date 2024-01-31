using CarWash.Api.Controllers.BaseController;
using CarWash.Entity.Dtos.Customer;
using CarWash.Entity.Dtos.WashPackage;
using CarWash.Service.Services.WashPackageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WashesController : CustomControllerBase
    {
        public readonly IWashPackageService _washPackageService;

        public WashesController(IWashPackageService washPackageService)
        {
            _washPackageService = washPackageService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] WashPackageDto requset)
        {
            var response = await _washPackageService.CreateWashPackage(requset);
            return CreateActionResultInstance(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePackage([FromBody] WashPackageDto requset)
        {
            var response = await _washPackageService.UpdateWashPackage(requset);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePackage([FromQuery] int id)
        {
            var response = await _washPackageService.DeleteWashPackage(id);
            return CreateActionResultInstance(response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> ListPackage()
        {
            var response = await _washPackageService.GetWashPackages();
            return CreateActionResultInstance(response);
        }
        
        [HttpGet("allForCustomer")]
        public async Task<IActionResult> GetAllPackageForCustomer()
        {
            var response = await _washPackageService.GetAllPackageForCustomer();
            return CreateActionResultInstance(response);
        }
    }
}
