namespace Net11Demo;

public class LinqJoins
{
    public LinqJoins()
    {
        List<(int Id, string Name, string? Category)> products =
        [
            (1, "Laptop", "Electronics"),
            (2, "Phone", "Electronics"),
            (3, "Orphan", null)
        ];
        List<(string Name, string Description)> categories =
        [
            ("Electronics", "Electronic devices"),
            ("Furniture", "Office furniture")
        ];

        // Before: GroupJoin + SelectMany + DefaultIfEmpty
        var oldLeftJoin = products
            .GroupJoin(categories, p => p.Category, c => c.Name, (p, cs) => (p, cs))
            .SelectMany(x => x.cs.DefaultIfEmpty(), (x, c) => $"{x.p.Name}: {c.Description ?? "(none)"}");
        Console.WriteLine(string.Join(", ", oldLeftJoin));

        Console.WriteLine("LeftJoin:");
        foreach (var (product, category) in products.LeftJoin(categories, p => p.Category, c => c.Name))
        {
            Console.WriteLine($"  {product.Name}: {category.Description ?? "(none)"}");
        }

        Console.WriteLine("RightJoin:");
        foreach (var (product, category) in products.RightJoin(categories, p => p.Category, c => c.Name))
        {
            Console.WriteLine($"  {product.Name ?? "(none)"}: {category.Description}");
        }

        Console.WriteLine("FullJoin:");
        foreach (var (product, category) in products.FullJoin(categories, p => p.Category, c => c.Name))
        {
            Console.WriteLine($"  {product.Name ?? "(none)"}: {category.Description ?? "(none)"}");
        }
    }
}
