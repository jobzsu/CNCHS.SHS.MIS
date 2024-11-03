using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.ModelConfigurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments", tbl =>
        {
            tbl.HasTrigger("Departments_Insert_Audit");
            tbl.HasTrigger("Departments_Update_Audit");
            tbl.HasTrigger("Departments_Delete_Audit");
        });

        builder.HasKey(x => x.Id)
            .IsClustered();
        builder.Property(x => x.Id)
            .IsRequired()
            .UseSequence("DepartmentIdSequence");

        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.Property(x => x.Name)
            .IsRequired();
    }
}
