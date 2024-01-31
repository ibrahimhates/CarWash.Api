using FluentValidation;
using FluentValidation.Results;

namespace CarWash.Core.Validation
{
    public class BaseValidator<T> : AbstractValidator<T>, IValidator<T> where T : class
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return context.InstanceToValidate == null
                ? new ValidationResult(new[] { new ValidationFailure("", "İstek içeriği doldurulmalıdır.") })
                : base.Validate(context);
        }
        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                context.AddFailure("İstek içeriği doldurulmalıdır.");
            }
            return base.PreValidate(context, result);

        }
    }
}
