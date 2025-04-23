namespace DDD.Demo.Infrastructure.Options;

public class RabbitMqOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string VirtualHost { get; set; } = "/";
}