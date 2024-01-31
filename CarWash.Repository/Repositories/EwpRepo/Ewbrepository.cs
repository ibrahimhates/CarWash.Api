using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using CarWash.Repository.Repositories.Employees;

namespace CarWash.Repository.Repositories.EwpRepo
{
    public class Ewbrepository : RepositoryBase<EmployeeWashProcess>, IEwbRepo
    {
        public Ewbrepository(AppDbContext context) : base(context)
        {
        }
    }
}
