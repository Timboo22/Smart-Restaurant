using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class ArtikelZutatConfiguration : IEntityTypeConfiguration<ArtikelZutat>
{
    public void Configure(EntityTypeBuilder<ArtikelZutat> entity)
    {
        entity.ToTable("artikel_zutaten");
        entity.HasKey(e => new { e.ArtikelId, e.ZutatId }).HasName("artikel_zutaten_pkey");
        entity.Property(e => e.ArtikelId).HasColumnName("artikel_id");
        entity.Property(e => e.ZutatId).HasColumnName("zutaten_id");
        entity.Property(e => e.Menge).HasColumnName("menge");
        entity.HasOne(e => e.Artikel)
            .WithMany(e => e.Zutaten)
            .HasForeignKey(e => e.ArtikelId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_artikel_zutaten_artikel");
        entity.HasOne(e => e.Zutat)
            .WithMany(e => e.Artikel)
            .HasForeignKey(e => e.ZutatId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_artikel_zutaten_zutat");
    }
}
