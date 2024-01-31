using CarWash.Core.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CarWash.Service.Services.ValidationService
{
    public class ValidationService
    {
        private static IServiceProvider _serviceProvider;
        private static ILogger<ValidationService> _logger;

        public static void SetProvider(IServiceCollection services, ILogger<ValidationService> logger)
        {
            _logger = logger;
            _serviceProvider = services.BuildServiceProvider();
        }
        public static ValidationResultModel ValidateModel<T>(T model) where T : class
        {
            var validatonResult = new ValidationResultModel();
            try
            {
                if (_serviceProvider == null)
                    throw new ValidationException("Provider is not created");

                var validator = (BaseValidator<T>)_serviceProvider.GetService<IValidator<T>>();

                if (validator == null)
                    throw new ValidationException("Validator is not registered");

                var results = validator.Validate(model);
                if (!results.IsValid)
                    validatonResult.Errors = results.Errors.ToDictionary(x => x.PropertyName, y => y.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ValidateModel)} throw an exception. Exception Message: {ex.Message}", ex);
            }
            return validatonResult;
        }
    }
}
