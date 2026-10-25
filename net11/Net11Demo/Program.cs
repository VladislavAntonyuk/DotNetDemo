using Net11Demo;

Console.WriteLine("Runtime Async");
await new RuntimeAsyncDemo().Run();
Console.ReadLine();

#if NET11_0_OR_GREATER

Console.WriteLine("Process API");
await new ProcessApiDemo().Run();
Console.ReadLine();

Console.WriteLine("LINQ Joins");
new LinqJoins();
Console.ReadLine();

Console.WriteLine("Async Validation");
await new AsyncValidationDemo().Run();
Console.ReadLine();

Console.WriteLine("Libraries");
await new LibrariesDemo().Run();
Console.ReadLine();
#endif

Console.WriteLine("Collection Expression Arguments");
new CollectionExpressionArguments();
Console.ReadLine();

Console.WriteLine("Unions");
new Unions();
Console.ReadLine();

Console.WriteLine("Closed Hierarchies");
new ClosedHierarchies();
Console.ReadLine();

Console.WriteLine("Extension Indexers");
new ExtensionIndexers();
Console.ReadLine();

Console.WriteLine("Labeled break and continue");
new LabeledBreak();
Console.ReadLine();
