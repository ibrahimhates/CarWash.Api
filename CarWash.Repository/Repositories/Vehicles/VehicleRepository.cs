using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repository.Repositories.Vehicles;

public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
{
    public VehicleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Vehicle>> GetAllVehicles(int customerId, bool trackChanges = false)
    {
        var vehicles = await FindByCondition(x => x.CustomerId == customerId, trackChanges)
            .Include(x => x.Brand)
            .ToListAsync();

        return vehicles;
    }
}