using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class ElementLanguage: EntityBase, IEntityBase
{
    public long ElementId { get; set; }
    public long LanguageId { get; set; }
    public string DisplayName { get; set; } = "";
    public string Tooltip { get; set; } = "";
    public string Help { get; set; } = "";
    
    //
    
    public Element Element { get; set; } = null!;
    public Language Language { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ElementLanguage>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ElementId)
                .IsRequired();

            e.Property(p => p.LanguageId)
                .IsRequired();

            e.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(p => p.Tooltip)
                .HasMaxLength(500);

            e.Property(p => p.Help)
                .HasMaxLength(500);

            e.HasIndex(i => new { i.ElementId, i.LanguageId })
                .IsUnique();
            
            e.HasOne(o=>o.Language)
                .WithMany()
                .HasForeignKey(k=>k.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}