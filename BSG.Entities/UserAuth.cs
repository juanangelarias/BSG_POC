using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class UserAuth: EntityBase, IEntityBase
{
    public long UserId { get; set; }
    public long ElementId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsVisible { get; set; }

    public User User { get; set; } = null!;
    public Element Element { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserAuth>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.UserId)
                .IsRequired();

            e.Property(p => p.ElementId)
                .IsRequired();

            e.HasIndex(i => new { i.UserId, i.ElementId })
                .IsUnique();

            e.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Element)
                .WithMany()
                .HasForeignKey(k => k.ElementId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}