#!/usr/bin/env dotnet
#:include helpers.cs
#:include models/customer.cs

// File-based app split into several files - no .csproj needed.
// Run: dotnet run app.cs -e GREETING=Hi
var greeting = Environment.GetEnvironmentVariable("GREETING") ?? "Hello";
Console.WriteLine(Helpers.Format(greeting, new Customer("Olena", "Kyiv")));
