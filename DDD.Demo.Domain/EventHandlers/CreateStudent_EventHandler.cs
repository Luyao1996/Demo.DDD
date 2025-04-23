using DDD.Demo.Domain.Share.Dto.User;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Json;

namespace DDD.Demo.Domain.EventHandlers;

public class CreateStudent_EventHandler: ILocalEventHandler<UserDetail>,
    ITransientDependency
{
    public IJsonSerializer _JsonSerializer { get; set; }

    public async Task HandleEventAsync(UserDetail eventData)
    {
        Console.WriteLine($"创建了一个学生:{_JsonSerializer.Serialize(eventData)}");
    }
}