using System.Text.Json.Serialization;
using DDD.Demo.Domain.Share.Enums;

namespace DDD.Demo.Domain.Share.Dto.User.Student;

public class StudentInfo:BasicUser
{
    public string Number { get; set; }
    public EnumSchoolRole Role { get; private set; } = EnumSchoolRole.Student;
    
    public StudentInfo(BasicUser basicUser,string? number = null)
    {
        Name = basicUser.Name;
        Age = basicUser.Age;
        IsMan = basicUser.IsMan;
        number = number;
    }

    public UserInfo ToUserInfo()
    {
        return new UserInfo()
        {
            Name = Name,
            Age = Age,
            IsMan = IsMan,
            Role = Role
        };
    }
}