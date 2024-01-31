using CarWash.Service.Services.ValidationService;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CarWash.Service.ServiceExtensions
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection RegisterFluentValidationCommandValidators(this IServiceCollection services, ILogger<ValidationService> logger)
        {
            ValidationService.SetProvider(services, logger);
            return services;
        }
    }
}
