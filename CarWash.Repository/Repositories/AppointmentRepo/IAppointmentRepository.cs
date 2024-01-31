
using CarWash.Entity.Entities;
using CarWash.Repository.Repositories.BaseRepository;

namespace CarWash.Repository.Repositories.AppointmentRepo
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        Task<List<Appointment>> GetAppointmentByCustIdAsync(int custId);
        Task<List<Appointment>> GetAppointmentByEmpIdAsync(int empId);
        Task<List<Appointment>> GetAll();
    }
}
