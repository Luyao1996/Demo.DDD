using DDD.Demo.Infrastructure.ORM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace DDD.Demo.Infrastructure.ORM.DBContext;

public class UserDbContext:AbpDbContext<UserDbContext>
{
    public DbSet<User> Users { get; set; }
    public DbSet<StudentEnrollmentInfo> StudentEnrollmentInfo { get; set; }
    
    // ？ 参数的名字必须是 options。由于ABP提供了匿名对象参数，所以更改它是不可能的。
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("users");
            
            //Configure the base properties
            b.ConfigureByConvention();
            
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            b.Property(x => x.Number).HasColumnName("number").IsRequired();
            b.Property(x => x.Age).HasColumnName("age").IsRequired();
            b.Property(x => x.IsMan).HasColumnName("is_man").IsRequired();
            b.Property(x => x.Role).HasColumnName("role").IsRequired().HasConversion<int>();
            
            //这种自增方案存在几个问题：
            // 1. 不适合集群环境
            // 2. 难以做多数据库适配
            //b.Property(x => x.Number).UseMySqlComputedColumn()
            //    .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            //    .HasAnnotation("MySql:IdentitySeed", 1000)
            //    .HasAnnotation("MySql:IdentityIncrement", 1);
        });
        modelBuilder.Entity<StudentEnrollmentInfo>(b =>
        {
            b.ToTable("student_enrollment");
            
            //Configure the base properties
            b.ConfigureByConvention();
            
            b.HasKey(x => x.Id);
            b.Property(x => x.StudentId).HasColumnName("student_id").IsRequired().HasMaxLength(50);
            b.Property(x => x.Key).HasColumnName("key").IsRequired().HasMaxLength(50);
            b.Property(x => x.Type).HasColumnName("type").IsRequired().HasConversion<int>();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}