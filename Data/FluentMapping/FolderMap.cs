using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Models;

namespace Todo.Data.FluentMapping;

public class FolderMap : IEntityTypeConfiguration<FolderModel>
{
    public void Configure(EntityTypeBuilder<FolderModel> builder)
    {
        builder.ToTable("Folder");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnType(SqlDataTypes.NVARCHAR)
            .HasMaxLength(80);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.CreatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(x => x.UpdatedAt)
            .HasColumnType(SqlDataTypes.DATETIME2)
            .HasDefaultValue(DateTime.UtcNow);

        builder.HasMany(x => x.Lists)
            .WithOne("FolderId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
