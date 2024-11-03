using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class InstructorInfoConfiguration : IEntityTypeConfiguration<InstructorInfo>
{
    public void Configure(EntityTypeBuilder<InstructorInfo> builder)
    {
        builder.ToTable("InstructorInfos", tbl =>
        {
            tbl.HasTrigger("InstructorInfos_Insert_Audit");
            tbl.HasTrigger("InstructorInfos_Update_Audit");
            tbl.HasTrigger("InstructorInfos_Delete_Audit");
        });

        builder.HasIndex(x => x.EmployeeId)
            .IsUnique();
        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.Major)
            .IsRequired();

        builder.Property(x => x.ContactInformation)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithOne(x => x.InstructorInfo)
            .HasForeignKey(typeof(InstructorInfo), "UserId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.DepartmentId)
            .IsRequired();
        builder.HasOne(x => x.Department)
            .WithMany(x => x.Instructors)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
