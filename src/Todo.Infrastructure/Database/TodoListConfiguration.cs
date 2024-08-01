
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Model;

namespace Todo.Infrastructure.Database;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.ToTable("todo_list");
        builder.Property(list => list.Name).HasMaxLength(100);
        builder.HasMany(list => list.Items)
            .WithOne()
            .HasForeignKey(item => item.ListId);
        builder.Metadata
            .FindNavigation(nameof(TodoList.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
