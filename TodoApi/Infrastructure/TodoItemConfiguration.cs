using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Domain;

namespace TodoApi.Infrastructure;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(item => item.Title).HasMaxLength(100);
        builder.HasOne<TodoList>()
            .WithMany(list => list.Items)
            .HasForeignKey(item => item.ListId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(item => new { item.ListId, item.Position });
    }
}
