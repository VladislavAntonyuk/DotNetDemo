using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Net11Web;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Blog> Blogs => Set<Blog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // EF Core 11: complex types (here mapped to JSON) in a TPT hierarchy
        modelBuilder.Entity<Animal>()
            .UseTptMappingStrategy()
            .ComplexProperty(a => a.Details, b => b.ToJson());
        modelBuilder.Entity<Dog>();
        modelBuilder.Entity<Cat>();

        // Configure a nested member with a lambda chain
        modelBuilder.Entity<Animal>()
            .Property(a => a.Details.Vet)
            .HasMaxLength(200);

        // Index over a complex type member
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Address.PostalCode);

        // Value converter whose provider type equals the column type: EF Core 11 drops the no-op CAST
        modelBuilder.Entity<Customer>()
            .Property(c => c.Name)
            .HasConversion(v => v.Trim(), v => v);
    }

    public async Task SeedAsync()
    {
        await Database.EnsureDeletedAsync();
        await Database.EnsureCreatedAsync();

        var news = new BlogType { Name = "News" };
        Blogs.AddRange(
            new Blog { Title = ".NET 11 is here", Type = news, Posts = [new() { Title = "Runtime Async" }, new() { Title = "C# 15" }] },
            new Blog { Title = "EF Core 11", Type = news, Posts = [new() { Title = "Complex types" }] },
            new Blog { Title = "Empty blog", Type = news });

        Animals.AddRange(
            new Dog { Name = "Rex", Breed = "Shepherd", Details = new() { BirthDate = new(2020, 5, 1), Vet = "Dr. Kovalenko" } },
            new Cat { Name = "Tom", Lives = 9, Details = new() { BirthDate = new(2019, 1, 15) } });

        var olena = new Customer { Name = "Olena", Address = new() { City = "Kyiv", PostalCode = "01001" } };
        Customers.AddRange(olena, new Customer { Name = "Andrii", Address = new() { City = "Lviv", PostalCode = "79000" } });
        Orders.AddRange(new Order { Customer = olena, Total = 120 }, new Order { Total = 55 });

        await SaveChangesAsync();
    }
}

[ComplexType]
public class AnimalDetails
{
    public DateTime BirthDate { get; set; }
    public string? Vet { get; set; }
}

public abstract class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public required AnimalDetails Details { get; set; }
}

public class Dog : Animal
{
    public string Breed { get; set; } = "";
}

public class Cat : Animal
{
    public int Lives { get; set; }
}

[ComplexType]
public class Address
{
    public string City { get; set; } = "";
    public string PostalCode { get; set; } = "";
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public required Address Address { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}

public class BlogType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public BlogType? Type { get; set; }
    public List<Post> Posts { get; set; } = [];
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
}

// Collects the SQL executed during one request so endpoints can return it
public sealed class SqlCapture : DbCommandInterceptor
{
    public List<string> Commands { get; } = [];

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        Commands.Add(command.CommandText);
        return result;
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        Commands.Add(command.CommandText);
        return ValueTask.FromResult(result);
    }
}
