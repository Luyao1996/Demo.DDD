using DDD.Demo.Domain.Contract;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.OutputInterface;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Demo.Domain.AggregateRoots;

/// <summary>
/// 示例代码
/// </summary>
public partial class DemoAggr:DomainBasicAggregateRoot<DemoAggr>
{
    private DemoAggr() { }
    public static DemoAggr CreateNew(IServiceProvider isp)
    {
        var instance = new DemoAggr();
        instance.OptionBuilder = new OptionBuilder<DemoAggr>(instance);
        instance.Isp = isp;
        return instance;
    }
    
    public async Task<bool> DoAsync() // GetListAsync InsertAsync UpdateAsync DeleteAsync PageAsync ....
    {
        var dataAccess = Isp.GetRequiredService<IUserDataAccess>();
        // Do somthing 。。。
        var result = true;
        return result;
    }
}


public partial class DemoAggr
{
    public DemoAggr WithActions(EnrollmentInfo enrollmentInfo)
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
        });
        return this;
    }

    public DemoAggr WithSomethingManagement()
    {
        this.OptionBuilder.AddOption(async aggr =>
        {
        });
        return this;
    }
}