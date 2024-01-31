using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWash.Repository.EntityConfigurations;

public class WashProcessConfig : IEntityTypeConfiguration<WashProcess>
{
    public void Configure(EntityTypeBuilder<WashProcess> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.WashPackage)
            .WithMany(x => x.WashProcesses)
            .HasForeignKey(x => x.WashPackageId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}