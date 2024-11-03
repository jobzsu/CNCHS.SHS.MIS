using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class ExternalAcademicRecordConfiguration : IEntityTypeConfiguration<ExternalAcademicRecord>
{
    public void Configure(EntityTypeBuilder<ExternalAcademicRecord> builder)
    {
        builder.ToTable("ExternalAcademicRecords", tbl =>
        {
            tbl.HasTrigger("ExternalAcademicRecords_Insert_Audit");
            tbl.HasTrigger("ExternalAcademicRecords_Update_Audit");
            tbl.HasTrigger("ExternalAcademicRecords_Delete_Audit");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("ExternalAcademicRecordIdSequence");

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.SubjectName)
            .IsRequired();

        builder.Property(x => x.Semester)
            .IsRequired();

        builder.Property(x => x.AcademicYear)
            .IsRequired();

        builder.Property(x => x.StudentId)
            .IsRequired();
        builder.HasOne(x => x.Student)
            .WithMany(x => x.ExternalAcademicRecords)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
