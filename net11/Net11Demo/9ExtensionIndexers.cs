namespace Net11Demo;

public static class SequenceExtensions
{
    extension(IEnumerable<int> sequence)
    {
        public int this[int index] => sequence.ElementAt(index);
    }

    extension(ulong bitmask)
    {
        public bool this[int bit] => (bitmask & (1UL << bit)) != 0;
    }
}

public class ExtensionIndexers
{
    public ExtensionIndexers()
    {
        IEnumerable<int> numbers = Enumerable.Range(10, 5);
        Console.WriteLine($"numbers[2] = {numbers[2]}");

        ulong mask = 0b1010;
        Console.WriteLine($"mask[1] = {mask[1]}, mask[2] = {mask[2]}");
    }
}
