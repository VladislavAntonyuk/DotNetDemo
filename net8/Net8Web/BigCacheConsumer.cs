using Microsoft.Extensions.Caching.Memory;

namespace Net8Web;

class BigCacheConsumer([FromKeyedServices("big")] IMemoryCache cache)
{
    public object? GetData() => cache.Get("data");
}