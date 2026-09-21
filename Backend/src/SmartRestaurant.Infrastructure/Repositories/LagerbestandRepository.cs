using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Domain.Entities;
using SmartRestaurant.Infrastructure.Persistence;

namespace SmartRestaurant.Infrastructure.Repositories;

public sealed class LagerbestandRepository(SmartRestaurantDbContext dbContext) : ILagerbestandRepository
{
    public Task<List<Lagerbestand>> GetAllWithZutatAsync(CancellationToken cancellationToken = default) =>
        dbContext.Lagerbestaende
            .AsNoTracking()
            .Include(l => l.Zutat)
            .ToListAsync(cancellationToken);

    public Task<Lagerbestand?> GetByZutatIdAsync(int zutatId, CancellationToken cancellationToken = default) =>
        dbContext.Lagerbestaende
            .Include(l => l.Zutat)
            .FirstOrDefaultAsync(l => l.ZutatId == zutatId, cancellationToken);
}
