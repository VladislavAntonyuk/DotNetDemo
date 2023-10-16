using Microsoft.Extensions.Caching.Memory;

namespace Net8Web;

class SmallCacheConsumer(IServiceProvider keyedServiceProvider)
{
    public object? GetData() => keyedServiceProvider.GetRequiredKeyedService<IMemoryCache>("small").Get("data");
}