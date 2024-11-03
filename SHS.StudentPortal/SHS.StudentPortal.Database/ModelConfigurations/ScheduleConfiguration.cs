using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules", tbl =>
        {
            tbl.HasTrigger("Schedules_Insert_Audit");
            tbl.HasTrigger("Schedules_Update_Audit");
            tbl.HasTrigger("Schedules_Delete_Audit");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("ScheduleIdSequence");

        builder.Property(x => x.Day)
            .IsRequired();

        builder.Property(x => x.TimeStart)
            .IsRequired();

        builder.Property(x => x.TimeEnd)
            .IsRequired();

        builder.Property(x => x.RoomNumber)
            .IsRequired();

        builder.Property(x => x.InstructorId)
            .IsRequired();
        builder.HasOne(x => x.Instructor)
            .WithMany(x => x.Schedules)
            .HasForeignKey(x => x.InstructorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.SubjectId)
            .IsRequired();
        builder.HasOne(x => x.Subject)
            .WithMany(x => x.Schedules)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
