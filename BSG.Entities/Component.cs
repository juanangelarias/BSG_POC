using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Component: EntityBase, IEntityBase
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    
    //

    public List<Element> Elements { get; set; } = [];
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Component>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(p => p.Description)
                .HasMaxLength(250);

            e.HasIndex(i => i.Name)
                .IsUnique();
            
            e.HasMany(x=>x.Elements)
                .WithOne(o=> o.Component)
                .HasForeignKey(k=>k.ComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}