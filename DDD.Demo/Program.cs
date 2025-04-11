using DDD.Demo;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();

await builder.AddApplicationAsync<Module>();

var app = builder.Build();

await app.InitializeApplicationAsync();

app.MapGet("hello", () => "Hello World!");

await app.RunAsync();