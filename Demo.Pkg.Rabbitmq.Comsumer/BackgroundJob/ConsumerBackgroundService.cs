using System.Formats.Tar;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.Json;

namespace Demo.Pkg.Rabbitmq.Comsumer.BackgroundJob;

public class ConsumerBackgroundService:BackgroundService
{
    public IJsonSerializer _JsonSerializer { get; set; }
    
    private ConsumerModuleOptions Options;
    public ConsumerBackgroundService(IOptions<ConsumerModuleOptions> options)
    {
        this.Options = options.Value;
    }

    private ConnectionFactory? _factory;
    private IConnection? _connection;
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"ConsumerBackgroundService is executing. Options: {this._JsonSerializer.Serialize(Options)}");
        
        _factory = new ConnectionFactory() { HostName = Options.Host };
        _connection = await _factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await _channel.ExchangeDeclareAsync(exchange: Options.Exchange, type: ExchangeType.Direct, cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(queue:Options.QueueName, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue: Options.QueueName, exchange: Options.Exchange, routingKey: Options.RoutingKey, cancellationToken: stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine("recived msg: "+msg);
            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queue: Options.QueueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("ConsumerBackgroundService is stopping.");

        _channel?.Dispose();
        _connection?.Dispose();

        await base.StopAsync(stoppingToken);
    }

    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("ConsumerBackgroundService is starting.");
        
        await base.StartAsync(stoppingToken);
    }
}