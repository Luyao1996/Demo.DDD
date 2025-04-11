namespace DDD.Demo.Domain.Share.Dto;

public class PageRequest<T>
{
    public T Conditions { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}