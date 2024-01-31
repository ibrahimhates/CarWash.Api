using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWash.Repository.EntityConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
