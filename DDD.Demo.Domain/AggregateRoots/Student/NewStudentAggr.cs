using DDD.Demo.Domain.Contract;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.Enums;
using DDD.Demo.Domain.Share.OutputInterface;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace DDD.Demo.Domain.DomainService.Student;


public partial class NewStudentAggr:DomainBasicAggregateRoot<NewStudentAggr>
{
    private UserInfo UserInfo { get; set; }
    private StudentInfo Apply { get; set; }
    private EnrollmentInfo EnrollmentInfo { get; set; }
    private Func<UserDetail,Task>? EventBusManagement { get; set; }

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
        
        this.UserInfo = new UserInfo();
        this.UserInfo.Name = this.Apply.Name;
        this.UserInfo.Age = this.Apply.Age;
        this.UserInfo.IsMan = this.Apply.IsMan;
            
        this.EnrollmentInfo ??= new EnrollmentInfo("create by system default",EnumEnrollmentType.Offical);
        
        var userId = await dataAccess.NewAsync(UserInfo);
        //记录招生信息
        await enrollmentDataAccess.NewAsync(EnrollmentInfo,userId);

        if (EventBusManagement != null)
        {
            var userDetail = new UserDetail()
            {
                Age = this.UserInfo.Age,
                Id = userId,
                IsMan = this.UserInfo.IsMan,
                Name = this.UserInfo.Name,
                //Number = this.UserInfo.Number,
                Role = EnumSchoolRole.Student
            };
            
            await EventBusManagement(userDetail);
        }
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

    public NewStudentAggr WithEventBusManagement()
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
            aggr.EventBusManagement = async (userDetail) =>
            {
                //本地事件
                var localEventBus = aggr.Isp.GetRequiredService<ILocalEventBus>();
                await localEventBus.PublishAsync(userDetail);
                //分布式事件
                var mqDataAccess = aggr.Isp.GetRequiredService<IUserMqAccess>();
                await mqDataAccess.PublishMsgAsync(userDetail);
            };
        });
        return this;
    }
}