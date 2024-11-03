using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class StudentInfoConfiguration : IEntityTypeConfiguration<StudentInfo>
{
    public void Configure(EntityTypeBuilder<StudentInfo> builder)
    {
        builder.ToTable("StudentInfos", tbl =>
        {
            tbl.HasTrigger("StudentInfos_Insert_Audit");
            tbl.HasTrigger("StudentInfos_Update_Audit");
            tbl.HasTrigger("StudentInfos_Delete_Audit");
        });

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.HasIndex(x => x.IdNumber)
            .IsUnique();
        builder.Property(x => x.IdNumber)
            .IsRequired()
            .UseSequence("StudentIdNumberSequence");

        builder.Property(x => x.StudentStatus)
            .IsRequired();

        builder.Property(x => x.YearLevel)
            .IsRequired();

        builder.Property(x => x.SectionId)
            .IsRequired();
        builder.HasOne(x => x.Section)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.TrackAndStrand)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithOne(x => x.StudentInfo)
            .HasForeignKey(typeof(StudentInfo), "UserId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Gender)
            .IsRequired();

        builder.Property(x => x.DateOfBirth)
            .IsRequired();

        builder.Property(x => x.PlaceOfBirth)
            .IsRequired();

        builder.Property(x => x.Age)
            .IsRequired();

        builder.Property(x => x.CivilStatus)
            .IsRequired();

        builder.Property(x => x.Nationality)
            .IsRequired();

        builder.Property(x => x.Religion)
            .IsRequired();

        builder.Property(x => x.ContactInformation)
            .IsRequired();

        builder.Property(x => x.Address)
            .IsRequired();
    }
}
