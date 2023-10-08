// DateTime
using System.Formats.Tar;

var dateTime = DateTime.Now;
Console.WriteLine(dateTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff"));
Console.WriteLine(dateTime.Microsecond);
Console.WriteLine(dateTime.Nanosecond);

// tar
//TarFile.CreateFromDirectory(sourceDirectoryName: "/Users/vladislav/Projects/test", destinationFileName: "/Users/vladislav/Projects/test.tar", includeBaseDirectory: true);
//TarFile.ExtractToDirectory(sourceFileName: "/Users/vladislav/Projects/test.tar", destinationDirectoryName: "/Users/vladislav/Projects/test/", overwriteFiles: true);

var repeatSequence = new RepeatSequence();

for (int i = 0; i < 5; i++)
    Console.WriteLine(repeatSequence++);

var result = IGenericMath.Sum<int, int>(new int[5] { 1, 2, 3, 4, 5 });
var result2 = IGenericMath.Sum<double, int>(new double[5] { 1, 2, 3, 4, 5 });
Console.WriteLine(result);
Console.WriteLine(result2);

[TypeAttribute(typeof(string))]
string Method() => "test";

[GenericAttribute<string>()]
string Method2() => "test";

// New lines in string interpolations
var fruits = new[] { "Apple", "Banana", "Orange", "Grapefruit" };
var str = $"Third letters of fruits starts with B: {fruits
   .Where(fruit => fruit.StartsWith("B"))
   .Select(fruit => fruit[2])
   .FirstOrDefault()}";

// List patterns
var list = new[] {
    new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, // A
    new[] {1, 2, 3, 5, 7, 10}, // B
    new[] {0, 2, 3, 8, 10, 10}, // C
    new[] {3, 4, 10}, // D
    new[] {1, 4, 10, 15, 21, 3, 10}, // E
    new[] {-1, 2, 3, 5, -1, 10} // F
};
list.Where(x => x is [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]); // matches list A
list.Where(x => x is [_, 2, 3, _, _, 10]); // matches lists B, C, F
list.Where(x => x is [1, 2, .., 10]); // matches lists A, B
list.Where(x => x is [1, _, .., 10]); // matches lists A, B, E
list.Where(x => x is [< 0, ..]); // matches list F

// Raw string literals
var line2 = "line2 from another variable";

var str1 = """"
line1
{line2} - won't be replaced
line3
      """ line4 """
       
 line6
"""";
Console.WriteLine(str1);
var str2 = $"""
line1 from second string
{line2}
line3
""";
Console.WriteLine(str2);

var str3 = $$"""
using { in line 1}
{{line2}}
{line2} this won't be replaced
{{{line2}}} you can use it like this
""";
Console.WriteLine(str3);

// Required
var requiredDemo = new RequiredDemo();

// Auto default struct
var structDemo = new AutoDefaultStruct();
Console.ReadLine();

// Extended nameof scope
[MyAttribute(nameof(parameter))]
void M(int parameter) { }