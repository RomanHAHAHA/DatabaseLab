using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DatabaseLab.API.ValidationAttributes;

public class EmailAttribute(int minLength = 10) : ValidationAttribute
{
    private readonly int _minLength = minLength;

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        var email = value as string;

        if (string.IsNullOrWhiteSpace(email))
        {
            return new ValidationResult("Email is required.");
        }

        if (email.Length < _minLength)
        {
            return new ValidationResult($"Email must be at least {_minLength} characters long.");
        }

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return new ValidationResult("Email must contain a valid '@' and domain part.");
        }

        return ValidationResult.Success!;
    }
}