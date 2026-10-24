using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Net11Web;

public class CreateUserRequest
{
    [Required, EmailAddress, UniqueEmail(ErrorMessage = "{0} is already registered")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Range(18, 120, ErrorMessage = "The field {0} must be between {1} and {2}.")]
    [Display(Name = "Age")]
    public int Age { get; set; }
}

// Async rule: checks the "database" without blocking a thread
public sealed class UniqueEmailAttribute : AsyncValidationAttribute
{
    static readonly HashSet<string> ExistingEmails = new(StringComparer.OrdinalIgnoreCase) { "taken@demo.com" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException("Use IsValidAsync");

    protected override async Task<ValidationResult?> IsValidAsync(object? value, ValidationContext validationContext, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        return ExistingEmails.Contains(value as string ?? "")
            ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName), [validationContext.MemberName!])
            : ValidationResult.Success;
    }
}

// Minimal in-memory localizer: real apps use .resx files or a database-backed IStringLocalizerFactory
public sealed class UkrainianValidationLocalizer : IStringLocalizer
{
    static readonly Dictionary<string, string> Translations = new()
    {
        ["Email"] = "Електронна пошта",
        ["Age"] = "Вік",
        ["{0} is already registered"] = "{0} вже зареєстровано",
        ["The field {0} must be between {1} and {2}."] = "Поле {0} має бути від {1} до {2}."
    };

    public LocalizedString this[string name] =>
        new(name, Translations.GetValueOrDefault(name, name), resourceNotFound: !Translations.ContainsKey(name));

    public LocalizedString this[string name, params object[] arguments] =>
        new(name, string.Format(this[name].Value, arguments), this[name].ResourceNotFound);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
        Translations.Select(t => new LocalizedString(t.Key, t.Value));
}
