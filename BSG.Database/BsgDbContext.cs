﻿using BSG.BackEnd.Services;
using BSG.Entities;
using BSG.Entities.Base;
using BSG.Entities.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BSG.Database;

public class BsgDbContext(DbContextOptions<BsgDbContext> options, IDateConverterService dateConverterService)
    : DbContext(options)
{
    // C
    public DbSet<Component> Components { get; set; } = null!;
    // E
    public DbSet<Element> Elements { get; set; } = null!;
    // P
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductType> ProductTypes { get; set; } = null!;
    // U
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserPassword> UserPasswords { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var type in EntityTypes.GetEntityTypes<IConfigurableEntity>())
        {
            var instance = (IConfigurableEntity)Activator.CreateInstance(type)!;
            instance.OnModelCreating(modelBuilder);
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
                continue;

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(dateConverterService.DateConverter());
                else if (property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(dateConverterService.NullableDateConverter());
            }
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateEntityMetadata();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateEntityMetadata();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateEntityMetadata();
        return base.SaveChanges();
    }

    private void UpdateEntityMetadata()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(
                e =>
                    e.Entity is IEntityBase
                    && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        var userId = -1; // _userResolverService.GetUserId();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((IEntityBase)entityEntry.Entity).CreatedById = userId;
                ((IEntityBase)entityEntry.Entity).CreatedOn = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((IEntityBase)entityEntry.Entity).ModifiedById = userId;
                ((IEntityBase)entityEntry.Entity).ModifiedOn = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Deleted)
            {
                ((IEntityBase)entityEntry.Entity).ModifiedById = userId;
                ((IEntityBase)entityEntry.Entity).ModifiedOn = DateTime.UtcNow;
                entityEntry.State = EntityState.Modified;
            }
        }
    }
}