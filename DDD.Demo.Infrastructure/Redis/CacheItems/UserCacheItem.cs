using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Enums;
using DDD.Demo.Infrastructure.ORM.Entities;
using NUglify.JavaScript.Syntax;

namespace DDD.Demo.Infrastructure.Redis.CacheItems;

public class UserCacheItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// 学生编号 唯一的
    /// </summary>
    public long Number { get; set; }
    public int Age { get; set; }
    public bool IsMan { get; set; }
    public EnumSchoolRole Role { get; set; }
    public DateTime? CreationTime { get; set; }
    public Guid? CreatorId{ get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId{ get; set; }
    
    public static UserCacheItem BuildFromUser(User user)
    {
        var res = new UserCacheItem();
        res.Id = user.Id;
        res.Name = user.Name;
        res.Age = user.Age;
        res.Number = user.Number;
        res.Role = user.Role;
        res.IsMan = user.IsMan;
        res.CreationTime = user.CreationTime;
        res.CreatorId = user.CreatorId;
        res.LastModificationTime = user.LastModificationTime;
        res.LastModifierId = user.LastModifierId;
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