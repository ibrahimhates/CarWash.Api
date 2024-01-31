using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarWash.Repository.Repositories.Employees
{
    public class EmployeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string> GetEmployeeRole(int userId)
        {
            return await FindByCondition(e => e.UserId == userId, false)
                .Select(e => e.Role.RoleName)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Employee>> GetWorkersWithAvailableTimeSlots(DateTime startDateTime, DateTime endDateTime)
        {
            var startDayOfWeekByte = (byte)startDateTime.DayOfWeek;
            var availableWorkers = await FindByCondition(worker => worker.RoleId == (int)EmployeeRoles.Worker &&
                    !worker.EmployeeAttendance.OffDays.Contains((Days)startDayOfWeekByte) &&
                    worker.EmployeeAttendance.ClockInDate <= startDateTime.TimeOfDay &&
                    worker.EmployeeAttendance.ClockOutDate >= endDateTime.TimeOfDay &&
                    ((worker.EmployeeAttendance.BreakDurationBegin != null && worker.EmployeeAttendance.BreakDurationEnd != null) &&
                     !(startDateTime.TimeOfDay > worker.EmployeeAttendance.BreakDurationBegin && endDateTime.TimeOfDay < worker.EmployeeAttendance.BreakDurationEnd)),true)
                .ToListAsync();
            
            return availableWorkers;
        }
    }
}
