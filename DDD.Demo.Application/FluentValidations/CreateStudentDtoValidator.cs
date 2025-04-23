using DDD.Demo.Domain.Share.Dto.User.Student;
using FluentValidation;

namespace DDD.Demo.Application.FluentValidations;

/// <summary>
/// TODO ？没生效
/// </summary>
public class CreateStudentDtoValidator: AbstractValidator<NewStudentInput>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.StudentInfo.Age).InclusiveBetween(16, 22);
        RuleFor(x => x.EnrollmentInfo.Key).Length(3, 30);
    }
}