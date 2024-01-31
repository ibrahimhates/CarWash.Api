using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Appointment;

namespace CarWash.Service.Services.AppointmentServices
{
    public interface IAppointmentService
    {
        Task<Response<NoContent>> CreateAppointment(CreateAppointmentDto request);
        Task<Response<List<AppointmentListDto>>> GetAppointmentsByCustId(int custId);
        Task<Response<List<AppointmentListDto>>> GetAppointmentsByEmpId(int empId);
        Task<Response<NoContent>> DeleteAppointment(int id);
        Task<Response<NoContent>> Update(AppointmentListDto updatedAppointment);
        Task<Response<NoContent>> AppointmentByScore(AppointmentScoreDto scoreDto);
        Task<Response<List<AppointmentListDto>>> GetAll();
    }
}
