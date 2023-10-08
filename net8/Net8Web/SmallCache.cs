using Microsoft.Extensions.Caching.Memory;

public class SmallCache : IMemoryCache
{
    public void Dispose()
    {
    }

    public ICacheEntry CreateEntry(object key)
    {
        return null;
    }

    public void Remove(object key)
    {
    }

    public bool TryGetValue(object key, out object? value)
    {
        value = "data from small cache";
        return true;
    }
}