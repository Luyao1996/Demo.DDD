using DDD.Demo.Domain.DomainService.Student;
using DDD.Demo.Domain.Share.Dto;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.Enums;
using DDD.Demo.Domain.Share.InputInterface;
using Volo.Abp.Application.Dtos;

namespace DDD.Demo.Domain.DomainService;

public class StudentManager:Volo.Abp.Domain.Services.DomainService,IStudentManager
{
    public StudentAggrRoot StudentAggrRoot { get; set; }
    public async Task<Guid> NewAsync(NewStudentInput input)
    {
        var apply = new StudentInfo(input.StudentInfo);
        var aggr = await StudentAggrRoot.CreateNewStudentAggr(apply)
            .WithEnrollment(input.EnrollmentInfo)
            .WithUserInfoManagement()
            .BuildAync();
        return await aggr.DoAsync();
    }

    public async Task<List<UserDetail>> ListAsync(UserListInput input)
    {
        var aggr = await StudentAggrRoot.CreateListStudentAggr()
            .WithConditions(input)
            .BuildAync();
        return await aggr.GetListAsync();
    }

    public async Task<PagedResultDto<UserDetail>> GetPageAsync(PageRequest<UserListInput> input)
    {
        var aggr = await StudentAggrRoot.CreateListStudentAggr()
            .WithConditions(input.Conditions)
            .WithPageManagement(input.SkipCount,input.MaxResultCount)
            .BuildAync();
        return await aggr.PageAsync();
    }
}