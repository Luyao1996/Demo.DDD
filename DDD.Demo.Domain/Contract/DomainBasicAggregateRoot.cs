namespace DDD.Demo.Domain.Contract;

public abstract class DomainBasicAggregateRoot<T>(IServiceProvider? isp=null)
{
    protected  OptionBuilder<T> OptionBuilder { get; set; }
    public IServiceProvider Isp { get; set; } = isp;

    public async Task<T> BuildAync()
    {
        return await OptionBuilder.BuildAsync();
    }
}