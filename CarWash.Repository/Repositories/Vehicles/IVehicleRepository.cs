using CarWash.Entity.Entities;
using CarWash.Repository.Repositories.BaseRepository;

namespace CarWash.Repository.Repositories.Vehicles;

public interface IVehicleRepository : IRepositoryBase<Vehicle>
{
    Task<IEnumerable<Vehicle>> GetAllVehicles(int customerId, bool trackChanges = false);
}