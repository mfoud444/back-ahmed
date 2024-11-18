using System.ComponentModel.DataAnnotations;

namespace Backend_Teamwork.src.Utils
{
    public class AtLeastOneRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            var properties = validationContext.ObjectType.GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(validationContext.ObjectInstance);
                if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue.ToString()))
                {
                    return ValidationResult.Success; // At least one property is set
                }
            }
            return new ValidationResult("At least one property must be updated.");
        }
    }
}
