
namespace Demo.Pkg.Rabbitmq.Comsumer;

public class ConsumerModuleOptions
{
    public string Host { get; set; }
    public string Exchange { get; set; }

    public string RoutingKey { get; set; }

    public string QueueName { get; set; }
}