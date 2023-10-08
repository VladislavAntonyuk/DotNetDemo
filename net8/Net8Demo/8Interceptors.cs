using System.Runtime.CompilerServices;

public class InterceptorsDemo
{
	public InterceptorsDemo()
    {
        Console.WriteLine("Demo");
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed class InterceptsLocationAttribute(string filePath, int line, int column) : Attribute
    {
    }
}

static class Interception
{
    [InterceptsLocation(@"C:\Projects\DotNetDemo\net8\Net8Demo\8Interceptors.cs", line: 7, column: 17)]
    public static void WriteLine(string? value)
    {
        Console.WriteLine($"Intercepted '{value}'");
    }
}