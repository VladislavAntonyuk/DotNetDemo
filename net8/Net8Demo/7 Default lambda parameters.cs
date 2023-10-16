namespace Net8Demo;

public class Lambdas
{
    public Lambdas()
    {
        var IncrementBy = (int source, int increment = 1) => source + increment;

        Console.WriteLine(IncrementBy(5)); // 6
        Console.WriteLine(IncrementBy(5, 2)); // 7
    }
}
