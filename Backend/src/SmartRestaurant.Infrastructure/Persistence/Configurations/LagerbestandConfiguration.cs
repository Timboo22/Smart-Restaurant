using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class LagerbestandConfiguration : IEntityTypeConfiguration<Lagerbestand>
{
    public void Configure(EntityTypeBuilder<Lagerbestand> entity)
    {
        entity.ToTable("lager");
        entity.HasKey(e => e.Id).HasName("lager_pkey");
        entity.Property(e => e.Id).HasColumnName("zutaten_lager_id");
        entity.Property(e => e.ZutatId).HasColumnName("zutaten_id");
        entity.Property(e => e.Soll).HasColumnName("soll");
        entity.Property(e => e.Ist).HasColumnName("ist");
        entity.HasOne(e => e.Zutat)
            .WithMany(e => e.Lagerbestaende)
            .HasForeignKey(e => e.ZutatId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_lager_zutat");
    }
}
