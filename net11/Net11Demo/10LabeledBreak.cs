namespace Net11Demo;

public class LabeledBreak
{
    public LabeledBreak()
    {
        int[,] grid =
        {
            { 1, 2, 3 },
            { 4, -1, 6 },
            { 7, 8, 42 }
        };

        outer:
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] < 0)
                {
                    Console.WriteLine($"Row {row} is corrupted, skipping");
                    continue outer;
                }

                if (grid[row, col] == 42)
                {
                    Console.WriteLine($"Found 42 at [{row},{col}]");
                    break outer;
                }
            }

            Console.WriteLine($"Row {row} processed");
        }
    }
}
