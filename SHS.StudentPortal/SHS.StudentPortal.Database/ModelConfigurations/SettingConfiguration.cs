using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Settings", tbl =>
        {
            tbl.HasTrigger("Settings_Insert_Audit");
            tbl.HasTrigger("Settings_Update_Audit");
            tbl.HasTrigger("Settings_Delete_Audit");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.Property(x => x.Name)
            .IsRequired();
    }
}
