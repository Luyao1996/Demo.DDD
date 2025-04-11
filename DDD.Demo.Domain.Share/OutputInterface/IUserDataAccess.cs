using System.Linq.Expressions;
using DDD.Demo.Domain.Share.Dto.User;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Domain.Share.OutputInterface;

public interface IUserDataAccess
{
    Task<Guid> NewAsync(UserInfo userInfo);
    Task<List<UserDetail>> ListAsync(UserListInput input); 
    Task<PagedResultDto<UserDetail>> PageAsync(UserListInput userInfo,int skipCount,int maxResultCount);
}