using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Infrastructure.ORM.Entities;
using NUglify.JavaScript.Syntax;

namespace DDD.Demo.Infrastructure.Redis.CacheItems;

public class UserCacheItem:User
{
    public static UserCacheItem BuildFromUser(User user)
    {
        var res = new UserCacheItem();
        res.Id = user.Id;
        res.Name = user.Name;
        res.Age = user.Age;
        res.Number = user.Number;
        res.Role = user.Role;
        res.IsMan = user.IsMan;
        res.ConcurrencyStamp = user.ConcurrencyStamp;
        res.CreationTime = user.CreationTime;
        res.CreatorId = user.CreatorId;
        res.LastModificationTime = user.LastModificationTime;
        res.LastModifierId = user.LastModifierId;
        res.ExtraProperties = user.ExtraProperties;
        return res;
    }

    public User ToUser()
    {
        var res = new User();
        res.Name = this.Name;
        res.Age = this.Age;
        res.Number = this.Number;
        res.Role = this.Role;
        res.IsMan = this.IsMan;
        res.ConcurrencyStamp = this.ConcurrencyStamp;
        res.CreationTime = this.CreationTime;
        res.CreatorId = this.CreatorId;
        res.LastModificationTime = this.LastModificationTime;
        res.LastModifierId = this.LastModifierId;
        res.SetId(this.Id);
        return res;
    }

    public static List<UserCacheItem> BuildFromUsers(List<User> users)
    {
        return users.Select(it=>UserCacheItem.BuildFromUser(it)).ToList();
    }

    public static List<User> ToUsers(List<UserCacheItem> userCacheItems)
    {
        return userCacheItems.Select(it=>it.ToUser()).ToList();
    }
}