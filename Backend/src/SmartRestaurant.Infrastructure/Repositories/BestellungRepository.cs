using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Domain.Entities;
using SmartRestaurant.Infrastructure.Persistence;

namespace SmartRestaurant.Infrastructure.Repositories;

public sealed class BestellungRepository(SmartRestaurantDbContext dbContext) : IBestellungRepository
{
    public async Task AddAsync(Bestellung bestellung, CancellationToken cancellationToken = default) =>
        await dbContext.Bestellungen.AddAsync(bestellung, cancellationToken);

    public Task<Bestellung?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Bestellungen.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public Task<Bestellung?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Bestellungen
            .Include(b => b.Positionen).ThenInclude(p => p.Artikel)
            .Include(b => b.StatusLogs).ThenInclude(l => l.Mitarbeiter)
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public void AddStatusLog(StatusLog statusLog) => dbContext.StatusLogs.Add(statusLog);
}
