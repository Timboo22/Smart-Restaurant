using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Domain.Entities;
using SmartRestaurant.Infrastructure.Persistence;

namespace SmartRestaurant.Infrastructure.Repositories;

public sealed class MitarbeiterRepository(SmartRestaurantDbContext dbContext) : IMitarbeiterRepository
{
    public Task<List<Mitarbeiter>> GetAllAsync(CancellationToken cancellationToken = default) =>
        dbContext.Mitarbeiter.AsNoTracking().ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Mitarbeiter.AnyAsync(m => m.Id == id, cancellationToken);
}
