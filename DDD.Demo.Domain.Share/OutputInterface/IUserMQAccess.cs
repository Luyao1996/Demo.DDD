using DDD.Demo.Domain.Share.Dto.User;

namespace DDD.Demo.Domain.Share.OutputInterface;

public interface IUserMqAccess
{
    Task PublishMsgAsync(UserDetail user);
}