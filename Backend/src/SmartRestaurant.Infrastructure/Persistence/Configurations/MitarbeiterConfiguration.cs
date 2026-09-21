using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class MitarbeiterConfiguration : IEntityTypeConfiguration<Mitarbeiter>
{
    public void Configure(EntityTypeBuilder<Mitarbeiter> entity)
    {
        entity.ToTable("mitarbeiter");
        entity.HasKey(e => e.Id).HasName("mitarbeiter_pkey");
        entity.Property(e => e.Id).HasColumnName("mitarbeiter_id");
        entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        entity.Property(e => e.Benutzername).HasColumnName("benutzername").HasMaxLength(100).IsRequired();
        entity.Property(e => e.Rolle).HasColumnName("rolle").HasMaxLength(50).IsRequired();
        entity.HasIndex(e => e.Benutzername).IsUnique().HasDatabaseName("mitarbeiter_benutzername_key");
    }
}
