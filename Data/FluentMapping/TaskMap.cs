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
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(80);

        builder.Property(t => t.Done)
            .IsRequired()
            .HasColumnName("done")
            .HasColumnType(SqlDataTypes.BIT)
            .HasDefaultValue(false);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(250);

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValue(DateTime.Now.ToUniversalTime());

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValue(DateTime.Now.ToUniversalTime());

        // Relationships
        builder.HasOne(t => t.List)
            .WithMany(x => x.Tasks)
            .HasConstraintName("FK_Task_List");
    }
}
