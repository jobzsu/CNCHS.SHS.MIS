using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.ObjectId)
            .IsRequired();

        builder.Property(x => x.ObjectType)
            .IsRequired();

        builder.Property(x => x.Event)
            .IsRequired();

        builder.Property(x => x.ActionById)
            .IsRequired();

        builder.Property(x => x.OccurredOn)
            .IsRequired();

        builder.Property(x => x.Data)
            .IsRequired();
    }
}
