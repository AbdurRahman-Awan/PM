using System.ComponentModel.DataAnnotations;

namespace PM.WEB.Extensions
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var comparisonValue = validationContext
                .ObjectType
                .GetProperty(_comparisonProperty)
                .GetValue(validationContext.ObjectInstance, null);

            if (value is DateTime dateTimeValue && comparisonValue is DateTime dateTimeComparison)
            {
                if (dateTimeValue <= dateTimeComparison)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be greater than {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}