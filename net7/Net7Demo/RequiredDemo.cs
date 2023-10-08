// DateTime
using System.Diagnostics.CodeAnalysis;

public class RequiredDemo
{
    public required string RequiredString { get; set; }

    [SetsRequiredMembers]
    public RequiredDemo()
    {
        RequiredString = string.Empty;
    }
}
