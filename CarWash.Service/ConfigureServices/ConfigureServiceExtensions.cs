using CarWash.Repository.Repositories.Brands;
using CarWash.Repository.Repositories.EmployeeAttendances;
using CarWash.Repository.Repositories.Employees;
using CarWash.Repository.Repositories.Roles;
using CarWash.Repository.Repositories.ServiceReviews;
using CarWash.Repository.Repositories.Token;
using CarWash.Repository.Repositories.Users;
using CarWash.Repository.Repositories.Vehicles;
using CarWash.Service.Providers;
using CarWash.Service.Services.AppointmentServices;
using CarWash.Service.Services.Auth;
using CarWash.Service.Services.EmployeeServices;
using CarWash.Service.Services.UserServices;
using CarWash.Service.Services.VehicleServices;
using CarWash.Service.Services.WashPackageServices;
using Microsoft.Extensions.DependencyInjection;

namespace CarWash.Service.ConfigureServices
{
    public static class ConfigureServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<JwtGenerator>();
            services.AddScoped<PasswordHasher>();
            
            services.AddScoped<IEmployeeRepository, EmployeRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IServiceReviewRepository, ServiceReviewRepository>();
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IEmployeeService, EmployeService>();

            services.AddScoped<IEmployeeAttendanceRepository,EmpolyeeAttendanceRepository>();

            services.AddScoped<IWashPackageService, WashPackageService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IUserService, UserService>();


        }
    }
}
