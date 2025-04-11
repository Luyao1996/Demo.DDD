using DDD.Demo.Domain.Share.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace DDD.Demo.Infrastructure.ORM.Entities;

public class StudentEnrollmentInfo:AuditedAggregateRoot<Guid>
{
    public Guid StudentId { get; set; }
    public EnumEnrollmentType Type { get; set; }
    /// <summary>
    /// 渠道对应的key ， 比如某个老师的用户id
    /// </summary>
    public string Key { get; set; }
    
    public void SetId(Guid guid) => this.Id = guid;
}