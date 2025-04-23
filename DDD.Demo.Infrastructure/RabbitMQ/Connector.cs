using AsyncKeyedLock;
using DDD.Demo.Infrastructure.Locker;
using DDD.Demo.Infrastructure.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Infrastructure.RabbitMQ;

public class Connector:ISingletonDependency,IAsyncDisposable
{
    public IAsyncLockProvider _lockerProvider { get; set; }

    private readonly ConnectionFactory _factory;
    private IConnection? _connection;

    public Connector(IOptions<RabbitMqOptions> options)
    {
        this._factory = new ConnectionFactory
        {
            HostName = options.Value.Host,
            Port = options.Value.Port,
            UserName = options.Value.UserName,
            Password = options.Value.Password,
            VirtualHost = options.Value.VirtualHost,
        };
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection == null)
        {
            using (await  _lockerProvider.Locker.LockAsync("123"))
            {
                if (_connection == null)
                {
                    _connection = await _factory.CreateConnectionAsync();
                }
            }
        }
        
        return _connection;
    }


    public async ValueTask DisposeAsync()
    {
        if (_connection != null && _connection.IsOpen)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}