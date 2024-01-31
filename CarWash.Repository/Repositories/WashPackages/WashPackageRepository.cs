using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using CarWash.Repository.Repositories.Users;

namespace CarWash.Repository.Repositories.WashPackages
{
    public class WashPackageRepository : RepositoryBase<WashPackage>, IWashPackageRepository
    {
        public WashPackageRepository(AppDbContext context) : base(context)
        {
        }



    }
}
