using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Net11Web;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Async validation + localized messages for Minimal APIs
builder.Services.AddLocalization();
builder.Services.AddValidation(options =>
{
    options.LocalizerProvider = (_, _) => new UkrainianValidationLocalizer();
});

// OpenAPI 3.2 is the default document version in .NET 11
builder.Services.AddOpenApi();

// Native OpenTelemetry: subscribe to the built-in source, no instrumentation package
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddSource("Microsoft.AspNetCore")
        .AddConsoleExporter());

// Zstandard response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<ZstandardCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

// Short-lived bearer tokens so the SignalR refresh demo is visible
builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken(options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromMinutes(1);
        options.Events.OnMessageReceived = context =>
        {
            // WebSockets cannot send headers: SignalR passes the token in the query string
            if (context.Request.Path.StartsWithSegments("/hubs") && context.Request.Query.TryGetValue("access_token", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

builder.Services.AddScoped<SqlCapture>();
builder.Services.AddDbContext<AppDbContext>((services, options) => options
    .UseSqlite("Data Source=net11demo.db")
    .AddInterceptors(services.GetRequiredService<SqlCapture>()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().SeedAsync();
}

app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();

// --- Validation and [ShortCircuit] ---
app.MapPost("/users", (CreateUserRequest request) => TypedResults.Created($"/users/{request.Email}", request));

// Skips authentication, authorization and every other middleware after routing
app.MapGet("/healthz", [ShortCircuit] () => "Healthy");

// --- OpenAPI 3.2 ---
// HTTP QUERY: safe and idempotent like GET, but with a request body
app.MapMethods("/products/search", ["QUERY"], (ProductFilter filter) => Catalog.Search(filter));

// Binary file schema: type: string, format: binary
app.MapGet("/products/{id:int}/datasheet", (int id) =>
        TypedResults.File(System.Text.Encoding.UTF8.GetBytes($"Datasheet for product {id}"), "application/pdf", $"product-{id}.pdf"))
    .Produces<FileContentHttpResult>(contentType: "application/pdf");

// [Obsolete] becomes deprecated: true
app.MapGet("/v1/products", [Obsolete("Use QUERY /products/search")] () => Catalog.Products);

// C# 15 union result: anyOf in OpenAPI
app.MapPost("/checkout", (CheckoutRequest request) => request.Amount > 1000
    ? new CheckoutResponse(new PaymentDeclined("LIMIT", "Amount exceeds the limit"))
    : new CheckoutResponse(new OrderReceipt(Guid.NewGuid(), request.Amount)));

// Typed Server-Sent Events: itemSchema in OpenAPI 3.2
app.MapGet("/orders/live", (CancellationToken cancellationToken) =>
    TypedResults.ServerSentEvents(Catalog.StreamOrdersAsync(cancellationToken)));

// --- SignalR: authentication refresh + client-side cancellation ---
app.MapPost("/token", (string user) => TypedResults.SignIn(
    new ClaimsPrincipal(new ClaimsIdentity(
        [new Claim(ClaimTypes.NameIdentifier, user), new Claim(ClaimTypes.Name, user), new Claim("issued", DateTime.Now.ToString("HH:mm:ss.fff"))],
        BearerTokenDefaults.AuthenticationScheme)),
    authenticationScheme: BearerTokenDefaults.AuthenticationScheme));

app.MapHub<PortfolioHub>("/hubs/portfolio", options =>
{
    options.EnableAuthenticationRefresh = true;
    options.CloseOnAuthenticationExpiration = true;
});

// Runs a .NET SignalR client against this server and reports what happened
app.MapGet("/signalr-demo", async (HttpContext context) =>
{
    var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
    using var http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    var log = new List<string>();

    await using var connection = new HubConnectionBuilder()
        .WithUrl($"{baseUrl}/hubs/portfolio", options => options.AccessTokenProvider = async () =>
        {
            var response = await http.PostAsync("/token?user=olena", null);
            var token = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
            log.Add($"Token requested, expires in {token!.ExpiresIn}s");
            return token.AccessToken;
        })
        .Build();

    await connection.StartAsync();
    log.Add($"Connected as {await connection.InvokeAsync<string>("WhoAmI")}");

    await Task.Delay(100);
    await connection.RefreshAuthenticationAsync();
    log.Add($"Refreshed without reconnect: {await connection.InvokeAsync<string>("WhoAmI")}");

    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
    try
    {
        await connection.InvokeAsync<string>("CalculateRisk", 10, cts.Token);
    }
    catch (OperationCanceledException)
    {
        log.Add("Client cancelled CalculateRisk after 1s");
    }

    await Task.Delay(200);
    log.Add($"Server observed the cancellation: {PortfolioHub.LastCalculationCancelled}");
    return log;
});

// --- EF Core 11 ---
app.MapGet("/efcore/animals", async (AppDbContext db, SqlCapture sql) => new
{
    // TPT hierarchy with a JSON-mapped complex type
    Animals = (await db.Animals.OrderBy(a => a.Name).ToListAsync()).Cast<object>(),
    Sql = sql.Commands
});

app.MapGet("/efcore/split-query", async (AppDbContext db, SqlCapture sql) =>
{
    // EF Core 11: the Posts query no longer joins BlogType
    var blogs = await db.Blogs
        .Include(b => b.Type)
        .Include(b => b.Posts)
        .AsSplitQuery()
        .ToListAsync();
    return new { Blogs = blogs.Select(b => new { b.Title, Type = b.Type?.Name, Posts = b.Posts.Count }), Sql = sql.Commands };
});

app.MapGet("/efcore/no-op-cast", (AppDbContext db) =>
    // EF Core 11: no CAST around [Name] even though it has a value converter
    db.Customers.Where(c => c.Name.StartsWith("Ol")).ToQueryString());

app.MapGet("/efcore/linq", async (AppDbContext db, SqlCapture sql, CancellationToken cancellationToken) =>
{
    var topBlog = await db.Blogs.MaxByAsync(b => b.Posts.Count, cancellationToken);
    var audit = await db.Customers
        .FullJoin(db.Orders, c => c.Id, o => o.CustomerId, (c, o) => new { Customer = c == null ? null : c.Name, Order = o == null ? (decimal?)null : o.Total })
        .ToListAsync(cancellationToken);
    return new { TopBlog = topBlog?.Title, Audit = audit, Sql = sql.Commands };
});

app.Run();
