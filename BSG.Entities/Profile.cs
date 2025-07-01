using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Profile : EntityBase, IEntityBase
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public List<UserProfile> UserProfiles { get; set; } = [];
    public List<ProfileAuth> ProfileAuths { get; set; } = [];

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Profile>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            e.Property(p => p.Description)
                .HasMaxLength(500);

            e.HasIndex(i => i.Name)
                .IsUnique();

            e.HasMany(x => x.UserProfiles)
                .WithOne(o => o.Profile)
                .HasForeignKey(k => k.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.ProfileAuths)
                .WithOne(o => o.Profile)
                .HasForeignKey(k => k.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}