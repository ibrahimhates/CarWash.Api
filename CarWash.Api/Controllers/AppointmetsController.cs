using CarWash.Api.Controllers.BaseController;
using CarWash.Entity.Dtos.Appointment;
using CarWash.Service.Services.AppointmentServices;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppointmetsController : CustomControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmetsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("createAppointment")]
        public async Task<IActionResult> CustLogin([FromBody] CreateAppointmentDto request)
        {
            var response = await _appointmentService.CreateAppointment(request);
            return CreateActionResultInstance(response);
        }

        [HttpGet("getByCustId")]
        public async Task<IActionResult> GetByCustId([FromQuery] int custId)
        {
            var response = await _appointmentService.GetAppointmentsByCustId(custId);
            return CreateActionResultInstance(response);
        }
        [HttpGet("getByEmpId")]
        public async Task<IActionResult> GetByEmpId([FromQuery] int empId)
        {
            var response = await _appointmentService.GetAppointmentsByEmpId(empId);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int appointmentId)
        {
            var response = await _appointmentService.DeleteAppointment(appointmentId);
            return CreateActionResultInstance(response);
        }

        [HttpPost("createReview")]
        public async Task<IActionResult> CreateReviewByScore([FromBody] AppointmentScoreDto appointmentScoreDto)
        {
            var response = await _appointmentService.AppointmentByScore(appointmentScoreDto);
            return CreateActionResultInstance(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            var response = await _appointmentService.GetAll();
            return CreateActionResultInstance(response);
        }


    }
}
