using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Notification: EntityBase, IEntityBase
{
    public string SenderEmail { get; set; } = "";
    public DateTime EmissionTime { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public bool SendEmail { get; set; }
    public bool SendSms { get; set; }
    
    public List<NotificationProperty> Properties { get; set; } = [];
    public List<NotificationRecipient> Recipients { get; set; } = [];
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Notification>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.SenderEmail)
                .IsRequired()
                .HasMaxLength(150);

            e.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(p => p.Body)
                .HasMaxLength(500)
                .IsRequired();

            e.HasIndex(i => new { i.SenderEmail, i.EmissionTime });

            e.HasMany(x => x.Properties)
                .WithOne(o => o.Notification)
                .HasForeignKey(k => k.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);
            
            e.HasMany(x=>x.Recipients)
                .WithOne(o=>o.Notification)
                .HasForeignKey(k=>k.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}