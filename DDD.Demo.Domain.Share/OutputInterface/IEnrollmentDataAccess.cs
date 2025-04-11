using DDD.Demo.Domain.Share.Dto.User.Student;

namespace DDD.Demo.Domain.Share.OutputInterface;

public interface IEnrollmentDataAccess
{
    Task<Guid> NewAsync(EnrollmentInfo info,Guid userId);
}