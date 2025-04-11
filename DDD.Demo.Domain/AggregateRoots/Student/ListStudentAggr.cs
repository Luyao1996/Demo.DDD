using DDD.Demo.Domain.Contract;
using DDD.Demo.Domain.Share.Dto;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.Enums;
using DDD.Demo.Domain.Share.OutputInterface;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;

namespace DDD.Demo.Domain.DomainService.Student;

public partial class ListStudentAggr:DomainBasicAggregateRoot<ListStudentAggr>
{
    public UserListInput InputCondition { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount{ get; set; }
    private ListStudentAggr() { }
    
    public static ListStudentAggr CreateNew(IServiceProvider isp)
    {
        var instance = new ListStudentAggr();
        instance.OptionBuilder = new OptionBuilder<ListStudentAggr>(instance);
        instance.Isp = isp;
        return instance;
    }
    
    public async Task<List<UserDetail>> GetListAsync()
    {
        var dataAccess = Isp.GetRequiredService<IUserDataAccess>();
        return await dataAccess.ListAsync(InputCondition);
    }

    public async Task<PagedResultDto<UserDetail>> PageAsync()
    {
        var dataAccess = Isp.GetRequiredService<IUserDataAccess>();
        return await dataAccess.PageAsync(InputCondition,SkipCount,MaxResultCount);
    }
}


public partial class ListStudentAggr
{
    public ListStudentAggr WithConditions(UserListInput input)
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
            aggr.InputCondition = input;
            aggr.InputCondition.Roles = new List<EnumSchoolRole>() { EnumSchoolRole.Student };
        });
        return this;
    }

    public ListStudentAggr WithPageManagement(int skipCount,int maxResultCount)
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
            aggr.SkipCount = skipCount;
            aggr.MaxResultCount = maxResultCount;
        });
        return this;
    }
}