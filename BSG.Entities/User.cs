using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class User: EntityBase, IEntityBase
{
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    
    public bool IsEnabled { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsAdmin { get; set; }

    #region Not in DTO

    public string EmailToken { get; set; } = string.Empty;
    public DateTime? EmailTokenExpiration { get; set; }
    
    public List<UserPassword> Passwords { get; set; } = [];
    
    #endregion
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<User>( e =>
        {
            MapBaseEntityProperties( e );

            e.Property( p => p.Username )
                .HasMaxLength( 250 )
                .IsRequired();

            e.Property( p => p.FullName )
                .HasMaxLength( 100 )
                .IsRequired();

            e.Property( p => p.Email )
                .HasMaxLength( 250 )
                .IsRequired();

            e.Property( p => p.PhoneNumber )
                .HasMaxLength( 20 )
                .IsRequired(  );
            
            e.Property( p => p.MobileNumber )
                .HasMaxLength( 20 )
                .IsRequired(  );

            e.Property( p => p.EmailToken )
                .HasMaxLength( 250 );

            e.HasIndex( i => i.Username )
                .IsUnique();

            e.HasMany( x => x.Passwords )
                .WithOne( o => o.User )
                .HasForeignKey( k => k.UserId )
                .OnDelete( DeleteBehavior.Restrict );
        } );
    }
}