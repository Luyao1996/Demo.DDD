using System.Text.Json.Serialization;
using DDD.Demo.Domain.Share.Enums;

namespace DDD.Demo.Domain.Share.Dto.User;

public class UserListInput
{
    public string? Name { get; set; }
    /// <summary>
    /// 年龄范围 起始
    /// </summary>
    public int? AgeStart { get; set; }
    /// <summary>
    /// 年龄范围 结束
    /// </summary>
    public int? AgeEnd { get; set; }
    public bool? IsMan { get; set; }
    /// <summary>
    /// 角色
    /// </summary>
    [JsonIgnore]
    public List<EnumSchoolRole>? Roles { get; set; }
    public DateTime? CreationTimeStart { get; set; }
    public DateTime? CreationTimeEnd { get; set; }
}