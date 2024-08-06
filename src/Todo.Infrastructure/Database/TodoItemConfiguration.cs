using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Model;

namespace Todo.Infrastructure.Database;

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
