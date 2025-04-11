using DDD.Demo.Application.Contract.Contracts;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.InputInterface;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace DDD.Demo.Application.Service;

public class HelloAppService:ApplicationService,IHelloAppService
{
    public IStudentManager _StudentManager { get; set; }
    
    public async Task<string> SayHelloAsync()
    {
        return "hello world!";
    }
    
    /// <summary>
    /// 你好
    /// </summary>
    /// <param name="name">名字</param>
    /// <returns>你好，名字</returns>
    [HttpGet("sayHello")]
    public async Task<string> SayHelloAsync(string name)
    {
        return $"hello {name}!";
    }
}