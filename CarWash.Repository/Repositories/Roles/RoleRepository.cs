using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repository.Repositories.Roles
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

    }
}
