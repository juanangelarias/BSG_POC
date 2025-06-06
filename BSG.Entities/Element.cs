using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Element: EntityBase, IEntityBase
{
    public long ComponentId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Tooltip { get; set; } = "";
    public string Help { get; set; } = "";
    
    //

    public Component Component { get; set; } = null!;
    
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

            e.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(p => p.Tooltip)
                .HasMaxLength(500);

            e.Property(p => p.Help)
                .HasMaxLength(500);

            e.HasIndex(i => new { i.ComponentId, i.Code })
                .IsUnique();
        });
    }
}