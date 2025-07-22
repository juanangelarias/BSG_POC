using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Element: EntityBase, IEntityBase
{
    public long ComponentId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    
    //

    public Component Component { get; set; } = null!;
    public List<UserAuth> UserAuths { get; set; } = [];
    public List<ProfileAuth> ProfileAuths { get; set; } = [];
    public List<ElementLanguage> Languages { get; set; } = [];
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Element>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ComponentId)
                .IsRequired();

            e.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.HasIndex(i => new { i.ComponentId, i.Code })
                .IsUnique();

            e.HasMany(x => x.UserAuths)
                .WithOne(o => o.Element)
                .HasForeignKey(k => k.ElementId)
                .OnDelete(DeleteBehavior.Restrict);
            
            e.HasMany(x=>x.ProfileAuths)
                .WithOne(o=>o.Element)
                .HasForeignKey(k=>k.ElementId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Languages)
                .WithOne(o => o.Element)
                .HasForeignKey(k => k.ElementId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}