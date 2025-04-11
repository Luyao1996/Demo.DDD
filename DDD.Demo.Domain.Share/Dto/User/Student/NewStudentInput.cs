using System.ComponentModel.DataAnnotations;

namespace DDD.Demo.Domain.Share.Dto.User.Student;

public class NewStudentInput
{
    [Required]
    public BasicUser StudentInfo { get; set; }

    /// <summary>
    /// 来源渠道信息 默认官方
    /// </summary>
    public EnrollmentInfo? EnrollmentInfo { get; set; }
}