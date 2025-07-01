using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class UserProfile: EntityBase, IEntityBase
{
    public long UserId { get; set; }
    public long ProfileId { get; set; }

    public User User { get; set; } = null!;
    public Profile Profile { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserProfile>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.UserId)
                .IsRequired();

            e.Property(p => p.ProfileId)
                .IsRequired();

            e.HasIndex(i => new { i.UserId, i.ProfileId })
                .IsUnique();
        });
    }
}