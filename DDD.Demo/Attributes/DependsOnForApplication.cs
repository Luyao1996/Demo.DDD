using System.Diagnostics;
using ConsumerModule = Demo.Pkg.Rabbitmq.Comsumer.Module;
using Volo.Abp.Modularity;

namespace DDD.Demo.Attributes;

/// <summary>
/// 返回运行时模块依赖，根据不同配置返回不同模块依赖
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependsOnForApplicationAttribute: Attribute, IDependedTypesProvider
{
    public Type[] GetDependedTypes()
    {
        var serverModule = System.Environment.GetEnvironmentVariable("ServerMod") ?? "webapi";
        return serverModule switch
        {
            "consumer" => [typeof(ConsumerModule)],
            _ =>
            [
                typeof(Application.Module),
                typeof(Domain.Module),
                typeof(Infrastructure.Module)
            ]
        };
    }
}