using DDD.Demo.Domain.Share.Dto;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.InputInterface;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DDD.Demo.Application.Service;

public class StudentAppService:ApplicationService
{
    public IStudentManager _StudentManager { get; set; }
    
    public async Task<Guid> NewAsync(NewStudentInput input)
    {
        return await _StudentManager.NewAsync(input);
    }
    
    public async Task<List<UserDetail>> GetListAsync(UserListInput input)
    {
        return await _StudentManager.ListAsync(input);
    }

    public async Task<PagedResultDto<UserDetail>> PageAsync(PageRequest<UserListInput> input)
    {
        return await _StudentManager.GetPageAsync(input);
    }
}