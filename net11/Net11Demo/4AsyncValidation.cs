#if NET11_0_OR_GREATER
using System.ComponentModel.DataAnnotations;

namespace Net11Demo;

public class AsyncValidationDemo
{
    public async Task Run()
    {
        // Property rules run first (async attribute), object-level rules only when properties are valid
        await Validate(new Invoice { VatNumber = "UA0000", Amount = 100 });
        await Validate(new Invoice { VatNumber = "UA1234", Amount = -5 });
        await Validate(new Invoice { VatNumber = "UA1234", Amount = 100 });
    }

    static async Task Validate(Invoice invoice)
    {
        var results = new List<ValidationResult>();
        var isValid = await Validator.TryValidateObjectAsync(invoice, new ValidationContext(invoice), results, validateAllProperties: true);
        Console.WriteLine($"{invoice.VatNumber}, {invoice.Amount}: valid = {isValid}");
        foreach (var result in results)
        {
            Console.WriteLine($"  {string.Join(",", result.MemberNames)}: {result.ErrorMessage}");
        }
    }
}

public class Invoice : IAsyncValidatableObject
{
    [Required, ValidVatNumber]
    public string? VatNumber { get; set; }

    public decimal Amount { get; set; }

    // IAsyncValidatableObject extends IValidatableObject: the sync path is still required
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
        throw new InvalidOperationException("Use ValidateAsync");

    public async IAsyncEnumerable<ValidationResult> ValidateAsync(ValidationContext validationContext, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        if (Amount <= 0)
        {
            yield return new ValidationResult("Amount must be positive", [nameof(Amount)]);
        }
    }
}

public sealed class ValidVatNumberAttribute : AsyncValidationAttribute
{
    static readonly HashSet<string> Registry = ["UA1234", "PL5678"];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException("Use IsValidAsync");

    protected override async Task<ValidationResult?> IsValidAsync(object? value, ValidationContext validationContext, CancellationToken cancellationToken)
    {
        // Simulates a call to an external VAT registry
        await Task.Delay(50, cancellationToken);
        return Registry.Contains(value as string ?? "")
            ? ValidationResult.Success
            : new ValidationResult($"VAT number '{value}' not found in the registry", [validationContext.MemberName!]);
    }
}
#endif