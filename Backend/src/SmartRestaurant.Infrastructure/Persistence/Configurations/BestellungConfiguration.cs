using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class BestellungConfiguration : IEntityTypeConfiguration<Bestellung>
{
    public void Configure(EntityTypeBuilder<Bestellung> entity)
    {
        entity.ToTable("bestellung");
        entity.HasKey(e => e.Id).HasName("bestellung_pkey");
        entity.Property(e => e.Id).HasColumnName("bestellung_id");
        entity.Property(e => e.TischId).HasColumnName("tisch_id");
        entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        entity.Property(e => e.Zeitpunkt).HasColumnName("zeitpunkt").HasColumnType("timestamp without time zone");
        entity.HasOne(e => e.Tisch)
            .WithMany(e => e.Bestellungen)
            .HasForeignKey(e => e.TischId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_bestelung_tisch");
    }
}
