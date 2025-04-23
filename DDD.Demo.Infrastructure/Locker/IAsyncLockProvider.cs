using AsyncKeyedLock;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Infrastructure.Locker;

public interface IAsyncLockProvider
{
    AsyncKeyedLocker<string> Locker { get; }
}