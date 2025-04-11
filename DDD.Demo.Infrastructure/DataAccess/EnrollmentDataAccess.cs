using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.OutputInterface;
using DDD.Demo.Infrastructure.ORM.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace DDD.Demo.Infrastructure.DataAccess;

public class EnrollmentDataAccess:IEnrollmentDataAccess,ITransientDependency
{
    public IGuidGenerator _guidGenerator {get; set;}
    
    public IRepository<StudentEnrollmentInfo, Guid> EnrollmentRepository {get; set;}
    
    public async Task<Guid> NewAsync(EnrollmentInfo info, Guid userId)
    {
        var entity = new StudentEnrollmentInfo()
        {
            StudentId = userId,
            Type = info.Type,
            Key =info.Key
        };
        
        entity.SetId(_guidGenerator.Create());
        await EnrollmentRepository.InsertAsync(entity);
        return entity.Id;
    }
}