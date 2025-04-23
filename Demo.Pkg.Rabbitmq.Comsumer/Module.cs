using Demo.Pkg.Rabbitmq.Comsumer.BackgroundJob;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Demo.Pkg.Rabbitmq.Comsumer;

public class Module:AbpModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        // Register the consumer background service
        context.Services.AddOptions<ConsumerModuleOptions>()
            .BindConfiguration("RabbitMQ:Consumer")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        context.Services.AddHostedService<ConsumerBackgroundService>();
        return base.ConfigureServicesAsync(context);
    }
}