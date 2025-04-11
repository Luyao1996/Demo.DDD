using Castle.Core.Configuration;
using DDD.Demo.Infrastructure.ORM.DBContext;
using DDD.Demo.Infrastructure.UnitOfLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace DDD.Demo.Infrastructure;

[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class Module:AbpModule
{
    public IConfigurationManager configurationManager { get; set; }

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(UnitOfCache<>));
        
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(opts =>
            {
                opts.UseMySQL();
                //全局设置实体不跟踪
                //opts.DbContextOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        });
        
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "demo";
        });
        Configure<RedisCacheOptions>(options =>
        {
            options.InstanceName = "hello";
        });
        
        // Configure<AbpDbConnectionOptions>(options =>
        // {
        //     options.ConnectionStrings.Default = "Server=192.168.171.128;Port=3306;Database=ddddemo;User=root;Pwd=192232;pooling=true;sslmode=Required;AllowPublicKeyRetrieval=true;CharSet=utf8;convert zero datetime=True";
        //     options.ConnectionStrings["AbpPermissionManagement"] = "...";
        // });
        
        context.Services.AddAbpDbContext<UserDbContext>(options =>
        {
            //includeAllEntities: true 为dbcontext中的每个实体都创建默认仓储 
            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}