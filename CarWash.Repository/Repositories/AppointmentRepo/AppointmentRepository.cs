using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repository.Repositories.AppointmentRepo
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Appointment>> GetAppointmentByCustIdAsync(int custId)
        {
            var list = await FindByCondition(a=> a.CustomerId == custId,true)
                .Include(a=> a.Vehicle)
                .ThenInclude(a=> a.Brand)
                .Include(a=> a.WashPackage)
                .Include(a=> a.WashProcess)
                .ThenInclude(a=> a.ServiceReview).ToListAsync();

            return list;
        }

        public async Task<List<Appointment>> GetAppointmentByEmpIdAsync(int empId)
        {
            var list = await FindAll(true)
                .Include(a => a.Vehicle)
                .ThenInclude(a => a.Brand)
                .Include(a => a.WashPackage)
                .Include(a => a.WashProcess)
                .ThenInclude(a => a.ServiceReview)
                .Include(a => a.WashProcess).ThenInclude(a=> a.Employees)
                .Where(a => a.WashProcess.Employees.Any(e=> e.EmployeeId == empId) && a.WashProcess.CarWashStatus != Entity.Enums.CarWashStatus.Completed)
                .OrderByDescending(a => a.WashProcess.CarWashStatus)
                .ToListAsync();

            return list;
        }

        public async Task<List<Appointment>> GetAll()
        {
            var list = await FindAll(true)
                .Include(a => a.Vehicle)
                .ThenInclude(a => a.Brand)
                .Include(a => a.WashPackage)
                .Include(a => a.WashProcess)
                .ThenInclude(a => a.ServiceReview)
                .Include(a => a.WashProcess).ThenInclude(a => a.Employees)
                .OrderByDescending(a => a.WashProcess.CarWashStatus)
                .ToListAsync();

            return list;
        }
    }
}
