using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence.Configurations;

public sealed class StatusLogConfiguration : IEntityTypeConfiguration<StatusLog>
{
    public void Configure(EntityTypeBuilder<StatusLog> entity)
    {
        entity.ToTable("status_log");
        entity.HasKey(e => e.Id).HasName("status_log_pkey");
        entity.Property(e => e.Id).HasColumnName("status_log_id");
        entity.Property(e => e.BestellungId).HasColumnName("bestellung_id");
        entity.Property(e => e.MitarbeiterId).HasColumnName("mitarbeiter_id");
        entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        entity.Property(e => e.Zeitpunkt).HasColumnName("zeitpunkt").HasColumnType("timestamp without time zone");
        entity.HasOne(e => e.Bestellung)
            .WithMany(e => e.StatusLogs)
            .HasForeignKey(e => e.BestellungId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_status_log_bestellung");
        entity.HasOne(e => e.Mitarbeiter)
            .WithMany(e => e.StatusLogs)
            .HasForeignKey(e => e.MitarbeiterId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_status_log_mitarbeiter");
    }
}
