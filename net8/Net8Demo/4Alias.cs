using io = System.IO;
using c = System.Console;
using Measurement = (double val, double err);

class Aliases
{
    public Aliases()
    {
        var fs = new io.FileStream("demo.txt", FileMode.Create);
        
        // (double val, double err) m1 = (312, 0.5);
        Measurement m1 = (312, 0.5); 
        c.WriteLine($"A measurement of {m1.val} {m1.err}");
    }
}