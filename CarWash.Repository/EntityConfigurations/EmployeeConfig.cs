using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repository.EntityConfigurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Anahtarlar ve ilişki tanımlamaları
            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired();

            // Yabancı anahtar ilişkisi
            builder.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
