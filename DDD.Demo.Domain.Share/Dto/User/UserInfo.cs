using DDD.Demo.Domain.Share.Enums;

namespace DDD.Demo.Domain.Share.Dto.User;

public class UserInfo :  BasicUser
{
    public EnumSchoolRole Role { get; set; }
}