using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;

namespace DDD.Demo.Infrastructure.UnitOfLogic;

public class UnitOfCache<T> where T: class
{
    public IAbpLazyServiceProvider _isp { get; set; }
    private IDistributedCache<T> _distributedCache => _isp.LazyGetRequiredService<IDistributedCache<T>>();
    private IAbpDistributedLock _distributedLock => _isp.LazyGetRequiredService<IAbpDistributedLock>();

    public async Task<T> TryGetCacheData(string key, Func<Task<T>> func)
    {
        var data = await _distributedCache.GetAsync(key);
        if (data != null)
            return data;
        
        var result = await func();
        await _distributedCache.SetAsync(key, result);
        return result;
    }
    
    public async Task<T> TryGetCacheDataWithDistributedLocking(string key,string lockingKey, Func<Task<T>> func)
    {
        var data = await _distributedCache.GetAsync(key);
        if (data != null)
            return data;
        
        await using (var handle = await _distributedLock.TryAcquireAsync(lockingKey,TimeSpan.FromSeconds(2)))
        {
            if (handle != null)
            {
                data = await _distributedCache.GetAsync(key);
                if (data != null)
                    return data;
                
                var result = await func();
                await _distributedCache.SetAsync(key, result);
                return result;
            }
        }
        
        return null;
    }
    
    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}