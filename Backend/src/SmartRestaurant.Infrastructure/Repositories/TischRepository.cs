using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Domain.Entities;
using SmartRestaurant.Infrastructure.Persistence;

namespace SmartRestaurant.Infrastructure.Repositories;

public sealed class TischRepository(SmartRestaurantDbContext dbContext) : ITischRepository
{
    public Task<List<Tisch>> GetAllAsync(CancellationToken cancellationToken = default) =>
        dbContext.Tische.AsNoTracking().ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Tische.AnyAsync(t => t.Id == id, cancellationToken);

    public Task<Tisch?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Tische.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
}
