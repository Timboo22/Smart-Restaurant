using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class BestellpositionConfiguration : IEntityTypeConfiguration<Bestellposition>
{
    public void Configure(EntityTypeBuilder<Bestellposition> entity)
    {
        entity.ToTable("bestellposition");
        entity.HasKey(e => e.Id).HasName("bestellposition_pkey");
        entity.Property(e => e.Id).HasColumnName("bestellposition_id");
        entity.Property(e => e.BestellungId).HasColumnName("bestellung_id");
        entity.Property(e => e.ArtikelId).HasColumnName("artikel_id");
        entity.Property(e => e.Menge).HasColumnName("menge");
        entity.HasOne(e => e.Bestellung)
            .WithMany(e => e.Positionen)
            .HasForeignKey(e => e.BestellungId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_bestellposition_bestellung");
        entity.HasOne(e => e.Artikel)
            .WithMany(e => e.Bestellpositionen)
            .HasForeignKey(e => e.ArtikelId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_bestellposition_artikel");
    }
}
