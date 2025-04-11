using System.Linq.Expressions;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.OutputInterface;
using DDD.Demo.Infrastructure.ORM.Entities;
using DDD.Demo.Infrastructure.Redis;
using DDD.Demo.Infrastructure.Redis.CacheItems;
using DDD.Demo.Infrastructure.UnitOfLogic;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace DDD.Demo.Infrastructure.DataAccess;

public class UserDataAccess:IUserDataAccess,ITransientDependency
{
    public IGuidGenerator _guidGenerator {get; set;}
    public IDistributedIdGenerator _redisIdGenerator { get; set; }
    public IRepository<User, Guid> _repo {get; set;}
    public UserUnitManager _userUnitManager { get; set; }

    public async Task<Guid> NewAsync(UserInfo userInfo)
    {
        var user = new User() {
            Age = userInfo.Age,
            IsMan = userInfo.IsMan,
            Name = userInfo.Name,
            Role = userInfo.Role,
        };
        
        user.SetId(_guidGenerator.Create());
        user.Number = await _redisIdGenerator.GetNextIdAsync("userNumber");
        await _repo.InsertAsync(user);
        await _userUnitManager.RemoveCache_UserListAge_18_20();
        return user.Id;
    }

    public async Task<List<UserDetail>> ListAsync(UserListInput input)
    {
        var hotConditions = input!=null && input.AgeStart >= 18 && input.AgeEnd <= 20;
        
        var userInfos = await _userUnitManager.TryGetFromCache_UserListAsync(
            hotCondition: hotConditions, 
            funcDataQuery: async () => UserCacheItem.BuildFromUsers(await _repo.GetListAsync(hotConditions)), 
            funcDataFilter: async (data) => UserCacheItem.ToUsers(data.FindAll(_userUnitManager.PredicateBuilder(input))));

        if (userInfos == default)
        {
            var expr = _userUnitManager.ExpressionBuilder(input);
            userInfos = await _repo.GetListAsync(expr);
        }
        
        var result = new List<UserDetail>();
        foreach (var userInfo in userInfos)
        {
            result.Add(new UserDetail()
            {
                Id = userInfo.Id,
                Name = userInfo.Name,
                Age = userInfo.Age,
                IsMan = userInfo.IsMan,
                Role = userInfo.Role,
            });
        }
        
        return result;
    }

    public async Task<PagedResultDto<UserDetail>> PageAsync(UserListInput input,int skipCount,int maxResultCount)
    {
        var hotConditions = input!=null && input.AgeStart >= 18 && input.AgeEnd <= 20;
        var userInfos = await _userUnitManager.TryGetFromCache_UserListAsync(
            hotCondition: hotConditions, 
            funcDataQuery: async () => UserCacheItem.BuildFromUsers(await _repo.GetListAsync(hotConditions)), 
            funcDataFilter: async (data) => UserCacheItem.ToUsers(data.FindAll(_userUnitManager.PredicateBuilder(input))));

        var totalCount = 0;
        List<User> pagedItems = new List<User>();
        if (userInfos != null)
        {
            totalCount = userInfos.Count;
            pagedItems = userInfos
                .OrderByDescending(u=>u.CreationTime)
                .Skip(skipCount)
                .Take(maxResultCount).ToList();
        }
        else
        {
            var expr = _userUnitManager.ExpressionBuilder(input);
            var queryable = await _repo.GetQueryableAsync();
            queryable = queryable.Where(expr);
            await queryable.CountAsync();
            pagedItems = await queryable
                .OrderByDescending(u=>u.CreationTime)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        var pageItemsResult = new List<UserDetail>();
        foreach (var pagedItem in pagedItems)
        {
            pageItemsResult.Add(new UserDetail()
            {
                Age = pagedItem.Age,
                Id = pagedItem.Id,
                IsMan = pagedItem.IsMan,
                Name = pagedItem.Name,
                Number = pagedItem.Number,
                Role = pagedItem.Role,
            });
        }
        return new PagedResultDto<UserDetail>(totalCount, pageItemsResult.AsReadOnly());
    }
}