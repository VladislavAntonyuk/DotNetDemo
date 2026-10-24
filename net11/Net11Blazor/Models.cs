using System.ComponentModel.DataAnnotations;

namespace Net11Blazor;

public class ContactModel
{
    [Required, StringLength(30, MinimumLength = 2)]
    [Display(Name = "Full name")]
    public string? Name { get; set; }

    [Required, EmailAddress]
    [Display(Name = "Email address")]
    public string? Email { get; set; }

    [Range(18, 120)]
    [Display(Name = "Age")]
    public int Age { get; set; } = 18;
}

public class RegistrationModel
{
    [Required, UsernameAvailable]
    [Display(Name = "User name")]
    public string? Username { get; set; }
}

public sealed class UsernameAvailableAttribute : AsyncValidationAttribute
{
    static readonly HashSet<string> Taken = new(StringComparer.OrdinalIgnoreCase) { "admin", "olena", "andrii" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException("Use IsValidAsync");

    protected override async Task<ValidationResult?> IsValidAsync(object? value, ValidationContext validationContext, CancellationToken cancellationToken)
    {
        // Simulates a database lookup; cancelled automatically when the user keeps typing
        await Task.Delay(1000, cancellationToken);
        return Taken.Contains(value as string ?? "")
            ? new ValidationResult($"'{value}' is already taken", [validationContext.MemberName!])
            : ValidationResult.Success;
    }
}

public record LogEntry(int Id, string Message);
