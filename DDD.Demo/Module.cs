using System.Net.Mime;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DDD.Demo;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))] 
[DependsOn(typeof(Application.Module))] 
[DependsOn(typeof(Domain.Module))]
[DependsOn(typeof(Infrastructure.Module))]
public class Module : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        // 输出已注册的服务信息
        //Console.WriteLine("===== 注册的服务列表 =====");
        //foreach (var service in services)
        //{
        //    Console.WriteLine($"服务类型: {service.ServiceType.FullName}");
        //    Console.WriteLine($"实现类型: {service.ImplementationType?.FullName ?? "N/A"}");
        //    Console.WriteLine($"生命周期: {service.Lifetime}");
        //    Console.WriteLine("------------------------");
        //}
        
        base.ConfigureServices(context);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseRouting();
        app.UseConfiguredEndpoints();              
    }
}