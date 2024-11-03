using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("UserAccounts", tbl =>
        {
            tbl.HasTrigger("UserAccounts_Insert_Audit");
            tbl.HasTrigger("UserAccounts_Update_Audit");
            tbl.HasTrigger("UserAccounts_Delete_Audit");
        });

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.HasIndex(x => x.Username)
            .IsUnique();
        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.EmailAddress)
            .IsRequired()
            .HasMaxLength(50);
    }
}
