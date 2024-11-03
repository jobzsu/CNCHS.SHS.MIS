using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
    //    builder.ToTable("Courses", tbl =>
    //    {
    //        tbl.HasTrigger("Courses_Insert_Audit");
    //        tbl.HasTrigger("Courses_Update_Audit");
    //        tbl.HasTrigger("Courses_Delete_Audit");
    //    });

    //    builder.HasKey(x => x.Id)
    //        .IsClustered();
    //    builder.Property(x => x.Id)
    //        .IsRequired()
    //        .UseSequence("CourseIdSequence");

    //    builder.Property(x => x.Name)
    //        .IsRequired();
    //    builder.HasIndex(x => x.Name)
    //        .IsUnique();

    //    builder.Property(x => x.DepartmentId)
    //        .IsRequired();
    //    builder.HasOne(x => x.Department)
    //        .WithMany(x => x.Courses)
    //        .HasForeignKey(x => x.DepartmentId)
    //        .OnDelete(DeleteBehavior.NoAction);
    }
}
