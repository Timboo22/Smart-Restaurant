using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Models;

namespace SmartRestaurant.Api.Data;

public sealed class SmartRestaurantDbContext(DbContextOptions<SmartRestaurantDbContext> options)
    : DbContext(options)
{
    public DbSet<Mitarbeiter> Mitarbeiter => Set<Mitarbeiter>();
    public DbSet<Tisch> Tische => Set<Tisch>();
    public DbSet<Artikel> Artikel => Set<Artikel>();
    public DbSet<Zutat> Zutaten => Set<Zutat>();
    public DbSet<Bestellung> Bestellungen => Set<Bestellung>();
    public DbSet<StatusLog> StatusLogs => Set<StatusLog>();
    public DbSet<Bestellposition> Bestellpositionen => Set<Bestellposition>();
    public DbSet<ArtikelZutat> ArtikelZutaten => Set<ArtikelZutat>();
    public DbSet<Lagerbestand> Lagerbestaende => Set<Lagerbestand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mitarbeiter>(entity =>
        {
            entity.ToTable("mitarbeiter");
            entity.HasKey(e => e.Id).HasName("mitarbeiter_pkey");
            entity.Property(e => e.Id).HasColumnName("mitarbeiter_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Benutzername).HasColumnName("benutzername").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Rolle).HasColumnName("rolle").HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Benutzername).IsUnique().HasDatabaseName("mitarbeiter_benutzername_key");
        });

        modelBuilder.Entity<Tisch>(entity =>
        {
            entity.ToTable("tisch");
            entity.HasKey(e => e.Id).HasName("tisch_pkey");
            entity.Property(e => e.Id).HasColumnName("tisch_id");
            entity.Property(e => e.Plaetze).HasColumnName("plaetze");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Artikel>(entity =>
        {
            entity.ToTable("artikel");
            entity.HasKey(e => e.Id).HasName("artikel_pkey");
            entity.Property(e => e.Id).HasColumnName("artikel_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Preis).HasColumnName("preis").HasPrecision(10, 2);
            entity.Property(e => e.Kategorie).HasColumnName("kategorie").HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Zutat>(entity =>
        {
            entity.ToTable("zutaten");
            entity.HasKey(e => e.Id).HasName("zutaten_pkey");
            entity.Property(e => e.Id).HasColumnName("zutaten_id");
            entity.Property(e => e.Name).HasColumnName("zutaten_name").HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Bestellung>(entity =>
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
        });

        modelBuilder.Entity<StatusLog>(entity =>
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
        });

        modelBuilder.Entity<Bestellposition>(entity =>
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
        });

        modelBuilder.Entity<ArtikelZutat>(entity =>
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
        });

        modelBuilder.Entity<Lagerbestand>(entity =>
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
        });
    }
}
