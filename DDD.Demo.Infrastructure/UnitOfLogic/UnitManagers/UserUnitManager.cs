using System.Linq.Expressions;
using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Infrastructure.Maps;
using DDD.Demo.Infrastructure.ORM.Entities;
using DDD.Demo.Infrastructure.Redis.CacheItems;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace DDD.Demo.Infrastructure.UnitOfLogic;

public class UserUnitManager:ITransientDependency
{
    private UnitOfCache<List<UserCacheItem>> _unitOfCache;
    private IUnitOfWorkManager _unitOfWorkManager;

    public UserUnitManager(UnitOfCache<List<UserCacheItem>> unitOfCache, IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfCache = unitOfCache;
        _unitOfWorkManager = unitOfWorkManager;
    }
    
    public async Task<List<User>> TryGetFromCache_UserListAsync(bool hotCondition,Func<Task<List<UserCacheItem>>> funcDataQuery,Func<List<UserCacheItem>,Task<List<User>>> funcDataFilter)
    {
        if (hotCondition)
        {
            var data = await _unitOfCache.TryGetCacheDataWithDistributedLocking(CacheMap.User_list_Age_18_20,CacheMap.User_list_Age_18_20_Locker, funcDataQuery);
            return await funcDataFilter(data);
        }

        return default;
    }
    
    public async Task RemoveCache_UserListAge_18_20()
    {
        var funcRemoveCache = new Func<Task>( async () => await _unitOfCache.RemoveAsync(CacheMap.User_list_Age_18_20));
        if(_unitOfWorkManager.Current == null) await funcRemoveCache();
        else _unitOfWorkManager.Current.OnCompleted(funcRemoveCache);
    }

    public Expression<Func<User, bool>> ExpressionBuilder(UserListInput input)
    {
        var conditions = new List<Expression<Func<User, bool>>>
        {
            it => true,
            !string.IsNullOrWhiteSpace(input.Name) ? (it => it.Name.Contains(input.Name)) : null,
            input.AgeStart != null  && input.AgeEnd != null ? (it => it.Age >= input.AgeStart && it.Age <= input.AgeEnd) : null,
            input.IsMan != null ? (it => it.IsMan == input.IsMan) : null,
            input.CreationTimeStart != null && input.CreationTimeEnd != null ? (it => it.CreationTime >= input.CreationTimeStart && it.CreationTime <= input.CreationTimeEnd) : null
        }.Where(c => c != null);
        Expression<Func<User, bool>> predicate = conditions.Aggregate((current, next) => current.And(next));
        return predicate;
    }
    
    public Predicate<User> PredicateBuilder(UserListInput input)
    {
        var expr = ExpressionBuilder(input);
        Func<User, bool> func = expr.Compile();
        return new Predicate<User>(func);
    }
}