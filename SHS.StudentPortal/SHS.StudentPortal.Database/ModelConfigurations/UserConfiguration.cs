using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", tbl =>
        {
            tbl.HasTrigger("Users_Insert_Audit");
            tbl.HasTrigger("Users_Update_Audit");
            tbl.HasTrigger("Users_Delete_Audit");
        });

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.FirstName)
            .IsRequired();

        builder.Property(x => x.LastName)
            .IsRequired();

        builder.Property(x => x.RoleType)
            .IsRequired();

        builder.Property(x => x.UserAccountId)
            .IsRequired();
        builder.HasOne(x => x.UserAccount)
            .WithOne(x => x.User)
            .HasForeignKey(typeof(User), "UserAccountId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
