using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class StudentScheduleConfiguration : IEntityTypeConfiguration<StudentSchedule>
{
    public void Configure(EntityTypeBuilder<StudentSchedule> builder)
    {
        builder.ToTable("StudentSchedules", tbl =>
        {
            tbl.HasTrigger("StudentSchedules_Insert_Audit");
            tbl.HasTrigger("StudentSchedules_Update_Audit");
            tbl.HasTrigger("StudentSchedules_Delete_Audit");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("StudentScheduleIdSequence");

        builder.Property(x => x.StudentId)
            .IsRequired();
        builder.HasOne(x => x.Student)
            .WithMany(x => x.StudentSchedules)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.ScheduleId)
            .IsRequired();
        builder.HasOne(x => x.Schedule)
            .WithMany(x => x.StudentSchedules)
            .HasForeignKey(x => x.ScheduleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Semester)
            .IsRequired();

        builder.Property(x => x.AcademicYear)
            .IsRequired();
    }
}
