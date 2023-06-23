using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Models;

namespace Todo.Data.FluentMapping;

public class RoleMap : IEntityTypeConfiguration<RoleModel>
{
    public void Configure(EntityTypeBuilder<RoleModel> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(80);

        builder.Property(t => t.CreatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.UpdatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
