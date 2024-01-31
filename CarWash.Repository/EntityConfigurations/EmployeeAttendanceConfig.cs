using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWash.Repository.EntityConfigurations
{
    public class EmployeeAttendanceConfig : IEntityTypeConfiguration<EmployeeAttendance>
    {
        public void Configure(EntityTypeBuilder<EmployeeAttendance> builder)
        {
            // Anahtarlar ve ilişki tanımlamaları
            builder.HasKey(ea => ea.Id);

            // Employee ile birebir ilişki
            builder.HasOne(ea => ea.Employee)
                .WithOne(e => e.EmployeeAttendance)
                .HasForeignKey<EmployeeAttendance>(ea => ea.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Silme davranışını isteğinize göre ayarlayabilirsiniz

            // Diğer konfigürasyonlar buraya eklenebilir.
        }
    }
}
