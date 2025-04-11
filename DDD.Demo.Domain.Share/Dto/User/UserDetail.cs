using DDD.Demo.Domain.Share.Enums;

namespace DDD.Demo.Domain.Share.Dto.User;

public class UserDetail :  UserInfo
{
    public Guid Id { get; set; }
    public long Number { get; set; }
}