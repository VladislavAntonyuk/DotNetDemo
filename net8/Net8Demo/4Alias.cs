using io = System.IO;
var fs = new io.FileStream(...);


using c = System.Console; ...
c.WriteLine("Hello World");



// Create a tuple that has two doubles
(double val, double err) m1 = (312, 0.5);
Now you can give your tuple an alias and make it feel a bit like struct:
// Give this tuple an alias
using Measurement = (double val, double err)
...
// Create a tuple by name
Measurement m1 = (312, 0.5); Console.WriteLine($"A measurement of {m1.val
