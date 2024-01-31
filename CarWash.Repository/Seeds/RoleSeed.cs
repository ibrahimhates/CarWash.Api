using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWash.Repository.Seeds
{
    public class RoleSeed : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var baseRoles = new List<Role>()
            {
                new ()
                {
                  CreatedAt = DateTime.UtcNow,
                  UpdatedAt = DateTime.UtcNow,
                  Id = 1,
                  IsDeleted = false,
                  RoleName = "SuperAdmin"
                },
                new ()
                {
                  CreatedAt = DateTime.UtcNow,
                  UpdatedAt = DateTime.UtcNow,
                  Id = 2,
                  IsDeleted = false,
                  RoleName = "Manager"
                },
                new ()
                {
                  CreatedAt = DateTime.UtcNow,
                  UpdatedAt = DateTime.UtcNow,
                  Id = 3,
                  IsDeleted = false,
                  RoleName = "Worker"
                }
            };

            builder.HasData(baseRoles);
        }
    }
}
