using System.Runtime.CompilerServices;

namespace Net8Demo
{
    public class InterceptorsDemo
    {
        public InterceptorsDemo()
        {
            Console.WriteLine("Demo");
        }
    }

    static class Interception
    {
        [InterceptsLocation(@"C:\Projects\DotNetDemo\net8\Net8Demo\8Interceptors.cs", line: 9, column: 21)]
        public static void WriteLine(string? value)
        {
            Console.WriteLine($"Intercepted '{value}'");
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed class InterceptsLocationAttribute(string filePath, int line, int column) : Attribute;
}