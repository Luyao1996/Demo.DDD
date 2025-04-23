using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.OutputInterface;
using DDD.Demo.Infrastructure.RabbitMQ;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace DDD.Demo.Infrastructure.MQAccess;

public class UserMQAccess:IUserMqAccess,ITransientDependency
{
    public IRabbitMqProducer _RabbitMqProducer { get; set; }
    public IJsonSerializer _JsonSerializer { get; set; }
    
    public async Task PublishMsgAsync(UserDetail user)
    {
        await _RabbitMqProducer.PublishAsync(_JsonSerializer.Serialize(user), "UserQueue", "UserExchange","1_rtKey_user");
    }
}