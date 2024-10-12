using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class PhoneNumberAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        var phoneNumber = value as string;

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return new ValidationResult("Phone number is required.");
        }

        if (phoneNumber.Length != 10 || !Regex.IsMatch(phoneNumber, @"^\d{10}$"))
        {
            return new ValidationResult("Phone number must be exactly 10 digits.");
        }

        return ValidationResult.Success!;
    }
}
