using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWash.Repository.EntityConfigurations
{
    public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            // Anahtarlar ve ilişki tanımlamaları
            builder.HasKey(v => v.Id);

            builder.HasOne(v => v.Customer)
                            .WithMany(c => c.Vehicles)
                            .HasForeignKey(v => v.CustomerId)
                            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
