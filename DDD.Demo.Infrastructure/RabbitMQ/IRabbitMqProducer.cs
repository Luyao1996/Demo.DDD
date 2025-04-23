namespace DDD.Demo.Infrastructure.RabbitMQ;

public interface IRabbitMqProducer
{
    Task PublishAsync(string message,string queueName = "",string exchangeName = "",string routingKey = "");
}