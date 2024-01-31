using CarWash.Api.Controllers.BaseController;
using CarWash.Entity.Dtos.VehicleDtos;
using CarWash.Service.Services.VehicleServices;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class VehicleController : CustomControllerBase
{
    private readonly IVehicleService _service;

    public VehicleController(IVehicleService service)
    {
        _service = service;
    }

    [HttpGet("all/{id:int}")]
    public async Task<IActionResult> GetAllVehicle([FromRoute(Name = "id")] int id)
    {
        var vehicles = await _service
            .GetAllVehicles(id);

        return CreateActionResultInstance(vehicles);
    }
    
    [HttpGet("allForAppoint/{id:int}")]
    public async Task<IActionResult> GetAllVehicleForAppointment([FromRoute(Name = "id")] int id)
    {
        var vehicles = await _service
            .GetAllVehiclesForAppointment(id);

        return CreateActionResultInstance(vehicles);
    }

    [HttpGet("all/brands")]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await _service.GetAllBrands();

        return CreateActionResultInstance(brands);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateVehicle([FromBody] VehicleCreateDto createDto)
    {
        var response = await _service.CreateVehicle(createDto);

        return CreateActionResultInstance(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateVehicle(
        [FromRoute(Name = "id")] int id,
        [FromBody] VehicleUpdateDto update)
    {
        var response = await _service.UpdateVehicle(update);

        return CreateActionResultInstance(response);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteVehicle([FromRoute(Name = "id")]int id)
    {
        var response = await _service.DeleteVehicle(id);

        return CreateActionResultInstance(response);
    }
}