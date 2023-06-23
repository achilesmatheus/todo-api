using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Models;

namespace Todo.Data.FluentMapping;

public class UserMap : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(80);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnType(SqlDataTypes.NVARCHAR);

        builder.Property(t => t.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType(SqlDataTypes.NVARCHAR);

        builder.Property(t => t.CreatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.UpdatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
