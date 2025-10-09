using BSG.Common.Constants;
using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class NotificationPropertyDefinition: EntityBase, IEntityBase
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<NotificationPropertyDefinition>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            e.Property(p => p.Type)
                .HasMaxLength(15)
                .IsRequired();

            e.HasIndex(i => i.Name)
                .IsUnique();
        });
    }
}