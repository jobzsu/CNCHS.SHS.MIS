using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database;

public class AppDbContext : DbContext
{
   
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options: options) { }

    #region > DbSets
    public DbSet<AcademicRecord> AcademicRecords => Set<AcademicRecord>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<InstructorInfo> Instructors => Set<InstructorInfo>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public DbSet<PreRequisite> PreRequisites => Set<PreRequisite>();

    public DbSet<Schedule> Schedules => Set<Schedule>();

    public DbSet<Section> Sections => Set<Section>();

    public DbSet<StudentInfo> Students => Set<StudentInfo>();

    public DbSet<StudentSchedule> StudentSchedules => Set<StudentSchedule>();

    public DbSet<Subject> Subjects => Set<Subject>();

    public DbSet<User> Users => Set<User>();

    public DbSet<UserAccount> UserAccounts => Set<UserAccount>(); 

    public DbSet<ExternalAcademicRecord> ExternalAcademicRecords => Set<ExternalAcademicRecord>();

    public DbSet<Setting> Settings => Set<Setting>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence("StudentIdNumberSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence("DepartmentIdSequence")
            .StartsAt(1000)
            .IncrementsBy(1000);

        modelBuilder.HasSequence("SubjectIdSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence("ScheduleIdSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence("StudentScheduleIdSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence("PreRequisiteIdSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence("AcademicRecordIdSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence("ExternalAcademicRecordIdSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
