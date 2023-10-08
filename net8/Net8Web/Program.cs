using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=net8demo.db"));

builder.Services.AddSingleton<BigCacheConsumer>();
builder.Services.AddSingleton<SmallCacheConsumer>();

builder.Services.AddKeyedSingleton<IMemoryCache, BigCache>("big");
builder.Services.AddKeyedSingleton<IMemoryCache, SmallCache>("small");

builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/big", (BigCacheConsumer data) => data.GetData())
    .WithOpenApi();
app.MapGet("/small", (SmallCacheConsumer data) => data.GetData())
    .WithOpenApi();
app.MapGet("/auth", (HttpContext context) => context.User.Identity.Name)
    .RequireAuthorization()
    .WithOpenApi();

app.MapGet("/efcore", (AppDbContext dbContext) =>
    {
        var query = dbContext.Database.SqlQueryRaw<UserDbTime>("SELECT Id, datetime('now') as DateTime from AspNetUsers")
            .Where(x => x.Id.Length > 10);
        Console.WriteLine(query.ToQueryString());
        return query.ToList();
    })
    .WithOpenApi();

app.MapGroup("/account").MapIdentityApi<AppUser>();
app.Run();