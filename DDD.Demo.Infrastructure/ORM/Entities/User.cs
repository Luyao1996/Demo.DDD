using DDD.Demo.Domain.Share.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace DDD.Demo.Infrastructure.ORM.Entities;

public class User:AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    /// <summary>
    /// 学生编号 唯一的
    /// </summary>
    public long Number { get; set; }
    public int Age { get; set; }
    public bool IsMan { get; set; }
    public EnumSchoolRole Role { get; set; }
    
    public void SetId(Guid guid) => this.Id = guid;
}