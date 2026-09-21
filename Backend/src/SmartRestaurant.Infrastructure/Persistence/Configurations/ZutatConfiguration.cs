using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class ZutatConfiguration : IEntityTypeConfiguration<Zutat>
{
    public void Configure(EntityTypeBuilder<Zutat> entity)
    {
        entity.ToTable("zutaten");
        entity.HasKey(e => e.Id).HasName("zutaten_pkey");
        entity.Property(e => e.Id).HasColumnName("zutaten_id");
        entity.Property(e => e.Name).HasColumnName("zutaten_name").HasMaxLength(50).IsRequired();
    }
}
