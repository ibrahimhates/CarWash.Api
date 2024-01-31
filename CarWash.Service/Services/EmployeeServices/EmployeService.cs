using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Employee;
using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Repositories.EmployeeAttendances;
using CarWash.Repository.Repositories.Employees;
using CarWash.Repository.Repositories.ServiceReviews;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.Mapping;
using CarWash.Service.ServiceExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarWash.Service.Services.EmployeeServices
{
    public class EmployeService : IEmployeeService
    {
        private readonly ILogger<EmployeService> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAttendanceRepository _employeeAttendanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceReviewRepository _serviceReviewRepository;
        public EmployeService(ILogger<EmployeService> logger,
            IEmployeeRepository employeeRepository,
            IEmployeeAttendanceRepository employeeAttendanceRepository,
            IUnitOfWork unitOfWork,
            IServiceReviewRepository serviceReviewRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _employeeAttendanceRepository = employeeAttendanceRepository;
            _unitOfWork = unitOfWork;
            _serviceReviewRepository = serviceReviewRepository;
        }

        public async Task<Response<NoContent>> UpdateEmployeeAttendance(CreateEmployeeAttandaceDto request)
        {
            _logger.SendInformation(nameof(UpdateEmployeeAttendance), "Started");
            try
            {
                var isAvailable = await _employeeRepository.AnyAsync(e => e.UserId == request.EmployeeId);
                if (isAvailable == false)
                {
                    _logger.SendWarning("Employee not found",nameof(UpdateEmployeeAttendance));
                    return Response<NoContent>.Fail("Çalışan bulunamadı!", 400);
                }

                var updatedAttendance = await _employeeAttendanceRepository
                    .FindByCondition(ea=> ea.EmployeeId == request.EmployeeId)
                    .FirstOrDefaultAsync();
                
                updatedAttendance = ObjectMapper.Mapper.Map(request,updatedAttendance);

                _employeeAttendanceRepository.Update(updatedAttendance);
                await _unitOfWork.SaveChangesAsync();

                _logger.SendInformation(nameof(UpdateEmployeeAttendance), "Update successful");
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(UpdateEmployeeAttendance), ex.Message);
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<IEnumerable<EmployeeListDto>>> GetAllEmployee()
        {
            var employees = await _employeeRepository
                .FindAll()
                .Include(x => x.Role)
                .Include(x => x.User)
                .Include(x => x.EmployeeAttendance)
                .ToListAsync();

            var employeesDto = ObjectMapper.Mapper.Map<List<EmployeeListDto>>(employees);
            
            return Response<IEnumerable<EmployeeListDto>>.Success(employeesDto,200);
        }

        public async Task<Response<IEnumerable<EmployeeReportListDto>>> GetAllEmployeeRapor()
        {

            var employeereportListDto = await _employeeRepository
                .FindAll()
                .Include(x => x.Role)
                .Where(x => x.Role.RoleName == "Worker")
                .Include(x => x.User)
                .Include(x => x.WashProcesses)
                .ThenInclude(x => x.WashProcess)
                .ThenInclude(x => x.ServiceReview)
                .Include(x => x.WashProcesses)
                .ThenInclude(x => x.WashProcess)
                .ThenInclude(x => x.Appointment)
                .ThenInclude(x => x.WashPackage)
                .Select(x => new EmployeeReportListDto()
                {
                    UserId = x.UserId,
                    FullName = x.User.FullName,
                    WeeklyIncome = CalculateWeeklyInComing(x.WashProcesses.Select(x => x.WashProcess.Appointment)),
                    MonthlyIncome = CalculateMonthlyInComing(x.WashProcesses.Select(x => x.WashProcess.Appointment)),
                    TotalScore = CalculateTotalScore(x.WashProcesses.Select(x => x.WashProcess.ServiceReview))
                }).ToListAsync();


            return Response<IEnumerable<EmployeeReportListDto>>.Success(employeereportListDto, 200);
        }

        public async Task<Response<IEnumerable<EmployeeReportDetailListDto>>> GetAllEmployeeDetailRapor(int userId)
        {
            var employeeDetailReport = await _serviceReviewRepository
                .FindAll()
                .Include(x => x.WashProcess)
                .ThenInclude(x => x.WashPackage)
                .Include(x => x.WashProcess)
                .ThenInclude(x => x.Appointment)
                .ThenInclude(x => x.Vehicle)
                .Include(x => x.WashProcess)
                .ThenInclude(x => x.Appointment)
                .ThenInclude(x => x.Customer)
                .ThenInclude(x => x.User)
                .Include(x => x.WashProcess)
                .ThenInclude(x => x.Employees.Where(e => e.EmployeeId == userId))
                .Select(x => new EmployeeReportDetailListDto()
                {
                    Id = x.Id,
                    PackageName = x.WashProcess.WashPackage.PackageName,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    Amount = x.WashProcess.WashPackage.Price,
                    CustomerName = x.WashProcess.Employees.Select(x => x.Employee.User.FullName).First(),
                    PlateNumber = x.WashProcess.Appointment.Vehicle.PlateNumber
                }).ToListAsync();
            
            return Response<IEnumerable<EmployeeReportDetailListDto>>.Success(employeeDetailReport, 200);
        }

        private static float CalculateTotalScore(IEnumerable<ServiceReview> list)
        {
            float total = 0;
            int count = 0;
            foreach (var item in list)
            {
                total += (float)item.Rating;
                count++;
            }

            float avg = total;
            if(count != 0)
                avg = total / (float)count;

            return avg;
        }

        private static double CalculateMonthlyInComing(IEnumerable<Appointment> list)
        {
            var beginDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(-30);
            double monthlyInComing = 0;
            foreach (var item in list)
            {
                if (item.AppointmentDate.Date >= endDate.Date
                    && item.AppointmentDate.Date <= beginDate.Date)
                    monthlyInComing += item.WashPackage.Price;
            }

            return monthlyInComing;
        }
        
        private static double CalculateWeeklyInComing(IEnumerable<Appointment> list)
        {
            var beginDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(-7);
            double weeklyInComing = 0;
            foreach (var item in list)
            {
                if (item.AppointmentDate.Date >= endDate.Date
                    && item.AppointmentDate.Date <= beginDate.Date)
                    weeklyInComing += item.WashPackage.Price;
            }

            return weeklyInComing;
        }
        
    }
}
