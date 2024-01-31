using CarWash.Entity.Entities;
using CarWash.Repository.Context;
using CarWash.Repository.Repositories.BaseRepository;

namespace CarWash.Repository.Repositories.ServiceReviews;

public class ServiceReviewRepository : RepositoryBase<ServiceReview>, IServiceReviewRepository
{
    public ServiceReviewRepository(AppDbContext context) : base(context)
    {
    }
}