    using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("Sections", tbl =>
        {
            tbl.HasTrigger("Sections_Insert_Audit");
            tbl.HasTrigger("Sections_Update_Audit");
            tbl.HasTrigger("Sections_Delete_Audit");
        });

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.AdviserId)
            .IsRequired();
        builder.HasOne(x => x.Adviser)
            .WithOne(x => x.Section)
            .HasForeignKey(typeof(Section), "AdviserId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
