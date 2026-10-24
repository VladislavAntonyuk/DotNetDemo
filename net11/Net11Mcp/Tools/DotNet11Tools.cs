using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Exposes "business data" to AI agents: here, the .NET 11 feature catalog from the talk.
/// </summary>
internal class DotNet11Tools
{
    static readonly Dictionary<string, string[]> Features = new(StringComparer.OrdinalIgnoreCase)
    {
        ["runtime"] = ["Runtime Async", "Higher hardware baseline", "JIT branch elimination"],
        ["libraries"] = ["Process.RunAndCaptureTextAsync", "LINQ LeftJoin/RightJoin/FullJoin", "AsyncValidationAttribute", "StringStream", "ZstandardStream"],
        ["csharp"] = ["Collection expression arguments with(...)", "union types", "closed hierarchies", "Extension indexers", "Labeled break/continue"],
        ["aspnetcore"] = ["OpenAPI 3.2 and HTTP QUERY", "Async validation and [ShortCircuit]", "SignalR authentication refresh", "dotnet new mcpserver"],
        ["blazor"] = ["Client-side validation in Static SSR", "Virtualize with variable height", "Circuit pause", "Components.AI"],
        ["efcore"] = ["Complex types in TPT/TPC", "Leaner split queries", "FullJoin and MaxByAsync translation", "Vector search"],
        ["maui"] = ["CoreCLR by default", "XAML incremental Hot Reload", "Experimental macOS/Linux/WPF backends"]
    };

    [McpServerTool]
    [Description("Lists what is new in .NET 11 for a given area of the stack.")]
    public string[] GetDotNet11Features(
        [Description("Area: runtime, libraries, csharp, aspnetcore, blazor, efcore or maui")] string area)
    {
        return Features.TryGetValue(area, out var features)
            ? features
            : [$"Unknown area '{area}'. Use one of: {string.Join(", ", Features.Keys)}"];
    }
}
