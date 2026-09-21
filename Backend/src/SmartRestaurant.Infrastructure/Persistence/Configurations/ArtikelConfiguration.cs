using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class ArtikelConfiguration : IEntityTypeConfiguration<Artikel>
{
    public void Configure(EntityTypeBuilder<Artikel> entity)
    {
        entity.ToTable("artikel");
        entity.HasKey(e => e.Id).HasName("artikel_pkey");
        entity.Property(e => e.Id).HasColumnName("artikel_id");
        entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        entity.Property(e => e.Preis).HasColumnName("preis").HasPrecision(10, 2);
        entity.Property(e => e.Kategorie).HasColumnName("kategorie").HasMaxLength(50).IsRequired();
    }
}
