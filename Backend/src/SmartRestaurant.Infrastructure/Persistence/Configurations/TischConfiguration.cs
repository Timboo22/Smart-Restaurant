using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class TischConfiguration : IEntityTypeConfiguration<Tisch>
{
    public void Configure(EntityTypeBuilder<Tisch> entity)
    {
        entity.ToTable("tisch");
        entity.HasKey(e => e.Id).HasName("tisch_pkey");
        entity.Property(e => e.Id).HasColumnName("tisch_id");
        entity.Property(e => e.Plaetze).HasColumnName("plaetze");
        entity.Property(e => e.Status).HasColumnName("status");
    }
}
