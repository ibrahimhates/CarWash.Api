using CarWash.Repository.Repositories.AppointmentRepo;
using CarWash.Repository.Repositories.Customers;
using CarWash.Repository.Repositories.Employees;
using CarWash.Repository.Repositories.EwpRepo;
using CarWash.Repository.Repositories.Roles;
using CarWash.Repository.Repositories.Token;
using CarWash.Repository.Repositories.Users;
using CarWash.Repository.Repositories.WashPackages;
using Microsoft.Extensions.DependencyInjection;

namespace CarWash.Repository.ConfigureRepositories
{
    public static class ConfigureRepositoryExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IWashPackageRepository, WashPackageRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IEwbRepo,Ewbrepository>();

        }
    }
}