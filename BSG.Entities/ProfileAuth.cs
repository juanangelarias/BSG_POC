using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class ProfileAuth: EntityBase, IEntityBase
{
    public long ProfileId { get; set; }
    public long ElementId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsVisible { get; set; }

    public Profile Profile { get; set; } = null!;
    public Element Element { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ProfileAuth>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ProfileId)
                .IsRequired();

            e.Property(p => p.ElementId)
                .IsRequired();

            e.Property(p => p.IsEnabled)
                .IsRequired();

            e.Property(p => p.IsVisible)
                .IsRequired();

            e.HasIndex(i => new { i.ProfileId, i.ElementId })
                .IsUnique();
        });
    }
}