using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class AcademicRecordConfiguration : IEntityTypeConfiguration<AcademicRecord>
{
    public void Configure(EntityTypeBuilder<AcademicRecord> builder)
    {
        builder.ToTable("AcademicRecords", tbl =>
        {
            tbl.HasTrigger("AcademicRecords_Insert_Audit");
            tbl.HasTrigger("AcademicRecords_Update_Audit");
            tbl.HasTrigger("AcademicRecords_Delete_Audit");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("AcademicRecordIdSequence");

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.StudentId)
            .IsRequired();
        builder.HasOne(x => x.Student)
            .WithMany(x => x.AcademicRecords)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.SubjectId)
            .IsRequired();
        builder.HasOne(x => x.Subject)
            .WithMany(x => x.AcademicRecords)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
