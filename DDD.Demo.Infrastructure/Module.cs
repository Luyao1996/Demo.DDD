using Castle.Core.Configuration;
using DDD.Demo.Infrastructure.Options;
using DDD.Demo.Infrastructure.ORM.DBContext;
using DDD.Demo.Infrastructure.Redis.CacheItems;
using DDD.Demo.Infrastructure.UnitOfLogic;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using StackExchange.Redis;
using StackExchange.Redis.Profiling;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace DDD.Demo.Infrastructure;

[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
[DependsOn(typeof(AbpDistributedLockingModule))]
[DependsOn(typeof(AbpCachingStackExchangeRedisModule))]
public class Module:AbpModule
{
    public IConfiguration configuration { get; set; }

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        configuration = context.Services.GetConfiguration();
        
        context.Services.AddTransient(typeof(UnitOfCache<>));
        DBConfigurtion(context.Services);
        CacheConfiguration(context.Services);
        MQConfiguration(context.Services);
    }

    private void MQConfiguration(IServiceCollection services)
    {
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration("RabbitMQ")
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private void DBConfigurtion(IServiceCollection services)
    {
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(opts =>
            {
                opts.UseMySQL();
                //全局设置实体不跟踪
                //opts.DbContextOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        });
                
        // Configure<AbpDbConnectionOptions>(options =>
        // {
        //     options.ConnectionStrings.Default = "Server=192.168.171.128;Port=3306;Database=ddddemo;User=root;Pwd=192232;pooling=true;sslmode=Required;AllowPublicKeyRetrieval=true;CharSet=utf8;convert zero datetime=True";
        //     options.ConnectionStrings["AbpPermissionManagement"] = "...";
        // });
        
        services.AddAbpDbContext<UserDbContext>(options =>
        {
            //includeAllEntities: true 为dbcontext中的每个实体都创建默认仓储 
            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }

    private void CacheConfiguration(IServiceCollection services)
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "demo";
        });
        Configure<RedisCacheOptions>(options =>
        {
            options.InstanceName = "hello";
            //options.Configuration = "192.168.171.128:6379,password=luyao.192232";
        });

        services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            Console.WriteLine("redis connection string:");
            Console.WriteLine(configuration["Redis:Configuration"]);
            var connection = ConnectionMultiplexer
                .Connect(configuration["Redis:Configuration"]);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }
}