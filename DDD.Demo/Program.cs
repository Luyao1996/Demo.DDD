using DDD.Demo;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();

SetWebHostUrl(builder);
    
await builder.AddApplicationAsync<Module>();

var app = builder.Build();

await app.InitializeApplicationAsync();

app.MapGet("hello", () => "Hello World!");

await app.RunAsync();

void SetWebHostUrl(WebApplicationBuilder builder)
{
    var envServerMod = System.Environment.GetEnvironmentVariable("ServerMod") ?? "webapi";
    if(envServerMod == "webapi")
        builder.WebHost.UseUrls("http://[::]:5183");
}