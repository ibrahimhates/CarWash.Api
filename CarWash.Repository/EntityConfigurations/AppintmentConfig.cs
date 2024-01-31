using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWash.Repository.EntityConfigurations
{
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // Anahtarlar ve ilişki tanımlamaları
            builder.HasKey(a => a.Id);

            // Customer ile ilişki
            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Silme davranışını isteğinize göre ayarlayabilirsiniz

            // WashPackage ile ilişki
            builder.HasOne(a => a.WashPackage)
                .WithMany(c=> c.Appointments)
                .HasForeignKey(a => a.PackageId)
                .OnDelete(DeleteBehavior.Cascade); // Silme davranışını isteğinize göre ayarlayabilirsiniz

            // Vehicle ile iliski
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.WashProcess)
                .WithOne(x => x.Appointment)
                .HasForeignKey<WashProcess>(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
