namespace Net8Demo;

public class DiscountService
{
    public double CalculateDiscount()
    {
        var now = DateTime.UtcNow;

        return now.DayOfWeek switch
        {
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            DayOfWeek.Sunday => 7,
            _ => 0
        };
    }
}

public class TimeAbstraction2(TimeProvider timeProvider)
{
    public double CalculateDiscount()
    {
        var now = timeProvider.GetUtcNow();

        return now.DayOfWeek switch
        {
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            DayOfWeek.Sunday => 7,
            _ => 0
        };
    }
}

// dotnet add package Microsoft.Extensions.TimeProvider.Testing --version 8.0.0-rc.1.23453.1
// Microsoft.Extensions.Time.Testing.FakeTimeProvider