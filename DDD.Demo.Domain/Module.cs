using Volo.Abp.Modularity;

namespace DDD.Demo.Domain;

[DependsOn(typeof(Share.Module))]
public class Module:AbpModule
{
    
}