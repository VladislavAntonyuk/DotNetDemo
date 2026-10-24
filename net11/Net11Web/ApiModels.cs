using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

namespace Net11Web;

public record ProductFilter(string? Text, decimal? MaxPrice, string[]? Categories);
public record Product(int Id, string Name, decimal Price, string Category);

public record CheckoutRequest(decimal Amount);
public record OrderReceipt(Guid OrderId, decimal Total);
public record PaymentDeclined(string Code, string Reason);

// C# 15 union as an API contract: OpenAPI describes it as anyOf
public union CheckoutResponse(OrderReceipt, PaymentDeclined);

public record OrderEvent(int OrderId, string Status);

public static class Catalog
{
    public static readonly Product[] Products =
    [
        new(1, "Laptop", 1500, "Electronics"),
        new(2, "Phone", 900, "Electronics"),
        new(3, "Desk", 300, "Furniture"),
        new(4, "Chair", 150, "Furniture")
    ];

    public static IEnumerable<Product> Search(ProductFilter filter) => Products
        .Where(p => filter.Text is null || p.Name.Contains(filter.Text, StringComparison.OrdinalIgnoreCase))
        .Where(p => filter.MaxPrice is null || p.Price <= filter.MaxPrice)
        .Where(p => filter.Categories is null || filter.Categories.Contains(p.Category));

    public static async IAsyncEnumerable<SseItem<OrderEvent>> StreamOrdersAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string[] statuses = ["Created", "Paid", "Shipped", "Delivered"];
        for (var i = 0; i < statuses.Length; i++)
        {
            await Task.Delay(500, cancellationToken);
            yield return new SseItem<OrderEvent>(new OrderEvent(42, statuses[i])) { EventId = i.ToString() };
        }
    }
}
