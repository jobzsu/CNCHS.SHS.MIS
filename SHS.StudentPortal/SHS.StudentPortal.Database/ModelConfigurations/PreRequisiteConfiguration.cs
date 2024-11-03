using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class PreRequisiteConfiguration : IEntityTypeConfiguration<PreRequisite>
{
    public void Configure(EntityTypeBuilder<PreRequisite> builder)
    {
        builder.ToTable("PreRequisites", tbl =>
        {
            tbl.HasTrigger("PreRequisites_Insert_Audit");
            tbl.HasTrigger("PreRequisites_Update_Audit");
            tbl.HasTrigger("PreRequisites_Delete_Audit");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("PreRequisiteIdSequence");

        builder.Property(x => x.PreRequisiteSubjectId)
            .IsRequired();

        builder.Property(x => x.SubjectId)
            .IsRequired();
        builder.HasOne(x => x.Subject)
            .WithMany(x => x.PreRequisites)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
