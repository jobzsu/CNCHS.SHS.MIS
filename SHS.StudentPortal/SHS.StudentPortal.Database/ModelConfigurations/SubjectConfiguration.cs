using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("Subjects", tbl =>
        {
            tbl.HasTrigger("Subjects_Insert_Audit");
            tbl.HasTrigger("Subjects_Update_Audit");
            tbl.HasTrigger("Subjects_Delete_Audit");
        });

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("SubjectIdSequence");

        builder.HasIndex(x => x.Code)
            .IsUnique();
        builder.Property(x => x.Code)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Hours)
            .IsRequired();

        builder.Property(x => x.Days)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.Units)
            .IsRequired();

        builder.Property(x => x.Semester)
            .IsRequired();

        builder.Property(x => x.AcademicYear)
            .IsRequired();

        builder.Property(x => x.TrackAndStrand)
            .IsRequired();
    }
}
