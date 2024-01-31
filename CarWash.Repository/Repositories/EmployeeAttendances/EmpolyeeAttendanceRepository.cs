using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using CarWash.Repository.Repositories.Employees;

namespace CarWash.Repository.Repositories.EmployeeAttendances
{
    public class EmpolyeeAttendanceRepository : RepositoryBase<EmployeeAttendance>, IEmployeeAttendanceRepository
    {
        public EmpolyeeAttendanceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
