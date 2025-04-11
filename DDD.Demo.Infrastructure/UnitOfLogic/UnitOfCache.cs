using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Infrastructure.UnitOfLogic;

public class UnitOfCache<T> where T: class
{
    private IDistributedCache<T> _distributedCache;

    public UnitOfCache(IDistributedCache<T> distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> TryGetCacheData(string key, Func<Task<T>> func)
    {
        var data = await _distributedCache.GetAsync(key);
        if (data != null)
            return data;
        
        var result = await func();
        await _distributedCache.SetAsync(key, result);
        return result;
    }
    
    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}