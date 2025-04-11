using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DDD.Demo.Infrastructure.Redis;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Application.Redis;

public class RedisDistributedIdGenerator : IDistributedIdGenerator, ITransientDependency
{
    private readonly IDatabase _redis;
    private readonly ILogger<RedisDistributedIdGenerator> _logger;

    public RedisDistributedIdGenerator(IConfiguration configuration, ILogger<RedisDistributedIdGenerator> logger)
    {
        var redisConnStr = configuration["Redis:Configuration"];
        var connection = ConnectionMultiplexer.Connect(redisConnStr);
        _redis = connection.GetDatabase();
        _logger = logger;
    }

    public async Task<long> GetNextIdAsync(string prefix)
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        var key = $"{prefix}:{date}";

        var nextId = await _redis.StringIncrementAsync(key);
        await _redis.KeyExpireAsync(key, TimeSpan.FromDays(2));

        var fullId = long.Parse($"{date}{nextId.ToString().PadLeft(6, '0')}");
        _logger.LogDebug($"Generated ID for {prefix}: {fullId}");

        return fullId;
    }
}