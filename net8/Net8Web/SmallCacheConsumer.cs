using Microsoft.Extensions.Caching.Memory;

class SmallCacheConsumer(IServiceProvider keyedServiceProvider)
{
    public object? GetData() => keyedServiceProvider.GetRequiredKeyedService<IMemoryCache>("small").Get("data");
}