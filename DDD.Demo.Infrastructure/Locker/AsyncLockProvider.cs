using AsyncKeyedLock;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Infrastructure.Locker;

public class AsyncLockProvider:IAsyncLockProvider,ISingletonDependency
{
    public AsyncKeyedLocker<string> Locker { get; } = new();
}