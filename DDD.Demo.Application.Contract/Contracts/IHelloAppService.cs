namespace DDD.Demo.Application.Contract.Contracts;

public interface IHelloAppService
{
    Task<string> SayHelloAsync();
}