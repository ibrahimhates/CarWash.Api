using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Employee;
using CarWash.Entity.Entities;

namespace CarWash.Service.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<Response<NoContent>> UpdateEmployeeAttendance(CreateEmployeeAttandaceDto request);
        Task<Response<IEnumerable<EmployeeListDto>>> GetAllEmployee();
        Task<Response<IEnumerable<EmployeeReportListDto>>> GetAllEmployeeRapor();
        Task<Response<IEnumerable<EmployeeReportDetailListDto>>> GetAllEmployeeDetailRapor(int userId);
    }
}