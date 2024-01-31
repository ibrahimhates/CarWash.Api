using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;

namespace CarWash.Repository.Repositories.Token
{
    public class TokenRepository : RepositoryBase<UserToken>, ITokenRepository
    {
        public TokenRepository(AppDbContext context) : base(context)
        {
        }



    }
}
