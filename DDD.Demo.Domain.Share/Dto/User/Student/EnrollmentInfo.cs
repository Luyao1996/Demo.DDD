using DDD.Demo.Domain.Share.Enums;

namespace DDD.Demo.Domain.Share.Dto.User.Student;

public class EnrollmentInfo(string key, EnumEnrollmentType type)
{
    public string Key { get; set; } = key;
    public EnumEnrollmentType Type { get; set; } = type;
}