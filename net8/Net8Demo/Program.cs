var timeAbstraction = new TimeAbstraction2(TimeProvider.System);
Console.WriteLine("TimeAbstraction");
Console.WriteLine(timeAbstraction.CalculateDiscount());
Console.ReadLine();

Console.WriteLine("StreamZip Demo");
new StreamZipDemo();
Console.ReadLine();

Console.WriteLine("Aliases");
new Aliases();
Console.ReadLine();

Console.WriteLine("Primary Constructor");
new PrimaryCtor3("label");
Console.ReadLine();

Console.WriteLine("Collection Expressions");
new CollectionExpressions();
Console.ReadLine();

Console.WriteLine("Lambdas");
new Lambdas();
Console.ReadLine();

Console.WriteLine("Interceptors");
new InterceptorsDemo();
Console.ReadLine();