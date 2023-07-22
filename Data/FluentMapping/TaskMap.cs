using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Models;

namespace Todo.Data.FluentMapping;

public class TaskMap : IEntityTypeConfiguration<TaskModel>
{
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(80);

        builder.Property(t => t.Done)
            .IsRequired()
            .HasColumnType(SqlDataTypes.BIT)
            .HasDefaultValue(false);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(250);

        builder.Property(t => t.CreatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.UpdatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
