namespace Net11Demo;

public class CollectionExpressionArguments
{
    public CollectionExpressionArguments()
    {
        string[] raw = ["one", "two", "three"];

        // Before: new List<string>(capacity) { ... } - no collection expression possible
        List<string> names = [with(capacity: raw.Length * 2), .. raw, "four"];
        Console.WriteLine($"List capacity: {names.Capacity}, items: {string.Join(',', names)}");

        HashSet<string> tags = [with(StringComparer.OrdinalIgnoreCase), "DotNet", "DOTNET", "dotnet"];
        Console.WriteLine($"Case-insensitive set count: {tags.Count}");

        Dictionary<string, int> counts = [with(StringComparer.OrdinalIgnoreCase)];
        counts["Net"] = 11;
        Console.WriteLine($"counts[\"NET\"] = {counts["NET"]}");
    }
}
