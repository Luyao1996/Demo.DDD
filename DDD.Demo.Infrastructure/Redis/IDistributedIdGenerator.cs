namespace DDD.Demo.Infrastructure.Redis;

public interface IDistributedIdGenerator
{
    Task<long> GetNextIdAsync(string prefix);
}