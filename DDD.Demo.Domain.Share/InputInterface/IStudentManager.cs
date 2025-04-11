using DDD.Demo.Domain.Share.Dto;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using Volo.Abp.Application.Dtos;

namespace DDD.Demo.Domain.Share.InputInterface;

public interface IStudentManager
{
    Task<Guid> NewAsync(NewStudentInput userInfo);
    Task<List<UserDetail>> ListAsync(UserListInput input);
    Task<PagedResultDto<UserDetail>> GetPageAsync(PageRequest<UserListInput> input);
}