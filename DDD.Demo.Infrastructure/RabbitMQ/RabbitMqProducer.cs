using System.ComponentModel.DataAnnotations;
using System.Text;
using DDD.Demo.Infrastructure.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Infrastructure.RabbitMQ;

public class RabbitMqProducer:IRabbitMqProducer,ITransientDependency
{
    public Connector _connector { get; set; }

    public async Task PublishAsync([Required]string message,string queueName = "",string exchangeName = "",string routingKey = "")
    {
        if (string.IsNullOrWhiteSpace(queueName)) queueName = "defaultByDemo";
        if (string.IsNullOrWhiteSpace(exchangeName)) exchangeName = "defaultExchangeByDemo";
        if(string.IsNullOrWhiteSpace(routingKey)) routingKey = "defaultRoutingKeyByDemo";
        
        var connection = await _connector.GetConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        //声明队列： queue:队列名字 durable：是否持久化 exclusive:不需要排它 autoDelete：自动删除
        //await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        await channel.ExchangeDeclareAsync(exchange: exchangeName, ExchangeType.Direct);

        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);
    }
}