using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;

namespace CarWash.Repository.Repositories.Brands;

public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
{
    public BrandRepository(AppDbContext context) : base(context)
    {
    }
}