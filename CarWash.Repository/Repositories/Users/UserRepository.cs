using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;
using Microsoft.AspNetCore.Identity;

namespace CarWash.Repository.Repositories.Users
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository(AppDbContext context) : base(context)
        {
        }
        

    }
}
