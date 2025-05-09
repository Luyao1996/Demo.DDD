﻿using DDD.Demo.Application.BackgroundJobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace DDD.Demo.Application;

[DependsOn(typeof(AbpSwashbuckleModule))]
[DependsOn(typeof(Contract.Module))]
[DependsOn(typeof(AbpFluentValidationModule))] // 参数验证器
public class Module:AbpModule
{
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var services = context.Services;
        
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // 自动为所有 ApplicationService 生成 API 控制器
            options.ConventionalControllers.Create(typeof(Module).Assembly);
        });

        SwaggerConfiguration(services);
        AuditLoggingConfiguration(services);
        BackgroundJobsConfiguration(services);
        
        
        await base.ConfigureServicesAsync(context);
    }



    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        //... other configurations.

        app.UseStaticFiles();

        app.UseSwagger();
        app.UseAuditing();

        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API");
        });
        
    }
    
    private void SwaggerConfiguration(IServiceCollection services)
    {
        var xmlFile = Path.Combine( AppContext.BaseDirectory, $"DDD.Demo.Application.xml");
        Console.WriteLine(xmlFile);
        
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.HideAbpEndpoints();
                options.IncludeXmlComments(xmlFile);
            }
        );
    }
    
    
    private void AuditLoggingConfiguration(IServiceCollection services)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = true;
            //options.IsEnabledForGetRequests = false;
        });
    }
    
    private void BackgroundJobsConfiguration(IServiceCollection services)
    {
        services.AddHostedService<PrintNowTimeBackgroundJob>();
    }

}