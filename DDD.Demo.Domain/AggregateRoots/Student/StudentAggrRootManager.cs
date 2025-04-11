using DDD.Demo.Domain.Share.Dto.User;
using DDD.Demo.Domain.Share.Dto.User.Student;
using DDD.Demo.Domain.Share.InputInterface;
using DDD.Demo.Domain.Share.OutputInterface;
using Volo.Abp.DependencyInjection;

namespace DDD.Demo.Domain.DomainService.Student;

public class StudentAggrRoot:ITransientDependency
{
    public IServiceProvider ServiceProvider { get; set; }
    public NewStudentAggr CreateNewStudentAggr(StudentInfo apply)
    {
        return NewStudentAggr.CreateNew(ServiceProvider,apply);
    }

    public ListStudentAggr CreateListStudentAggr()
    {
        return ListStudentAggr.CreateNew(ServiceProvider);
    }
}