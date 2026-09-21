using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Infrastructure.Persistence;

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartRestaurantDbContext).Assembly);
    }
}
