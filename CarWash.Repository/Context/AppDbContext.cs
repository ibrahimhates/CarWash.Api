using CarWash.Core.Entity;
using CarWash.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace CarWash.Repository.Context
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<EmployeeWashProcess> EmployeeWashProcesses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceReview> ServiceReviews { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<WashPackage> WashPackages { get; set; }
        public DbSet<WashProcess> WashProcesses { get; set; }
        public DbSet<UserToken> Tokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasQueryFilter(e => e.IsDeleted != true);
            builder.Entity<WashPackage>()
                .HasQueryFilter(e => e.IsDeleted != true);
            builder.Entity<Appointment>()
                .HasQueryFilter(a => a.IsDeleted != true);
            builder.Entity<WashProcess>()
                .HasQueryFilter(a => a.IsDeleted != true);
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((EntityBase)entity.Entity).CreatedAt = now;
                }
                if (entity.State == EntityState.Modified)
                    ((EntityBase)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
