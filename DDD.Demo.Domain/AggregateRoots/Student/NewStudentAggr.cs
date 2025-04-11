using DDD.Demo.Domain.Contract;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.Enums;
using DDD.Demo.Domain.Share.OutputInterface;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Uow;

namespace DDD.Demo.Domain.DomainService.Student;


public partial class NewStudentAggr:DomainBasicAggregateRoot<NewStudentAggr>
{
    private UserInfo UserInfo { get; set; }
    private StudentInfo Apply { get; set; }
    private EnrollmentInfo EnrollmentInfo { get; set; }

    private NewStudentAggr() { }
    public static NewStudentAggr CreateNew(IServiceProvider isp,StudentInfo apply)
    {
        var instance = new NewStudentAggr();
        instance.OptionBuilder = new OptionBuilder<NewStudentAggr>(instance);
        instance.Isp = isp;
        instance.Apply = apply;
        return instance;
    }
    
    public async Task<Guid> DoAsync()
    {
        var dataAccess = Isp.GetRequiredService<IUserDataAccess>();
        var enrollmentDataAccess = Isp.GetRequiredService<IEnrollmentDataAccess>();
        
        var userId = await dataAccess.NewAsync(UserInfo);
        await enrollmentDataAccess.NewAsync(EnrollmentInfo,userId);
        
        return userId;
    }
}

public partial class NewStudentAggr
{
    public NewStudentAggr WithEnrollment(EnrollmentInfo enrollmentInfo)
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
            aggr.EnrollmentInfo = enrollmentInfo;
        });
        return this;
    }

    public NewStudentAggr WithUserInfoManagement()
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
            aggr.UserInfo = new UserInfo();
            aggr.UserInfo.Name = aggr.Apply.Name;
            aggr.UserInfo.Age = aggr.Apply.Age;
            aggr.UserInfo.IsMan = aggr.Apply.IsMan;
            
            aggr.EnrollmentInfo ??= new EnrollmentInfo("create by system default",EnumEnrollmentType.Offical);
        });
        return this;
    }
}