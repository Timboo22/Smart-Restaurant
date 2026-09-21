using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Domain.Entities;
using SmartRestaurant.Infrastructure.Persistence;

namespace SmartRestaurant.Infrastructure.Repositories;

public sealed class ArtikelRepository(SmartRestaurantDbContext dbContext) : IArtikelRepository
{
    public Task<List<Artikel>> GetAllWithZutatenAsync(CancellationToken cancellationToken = default) =>
        dbContext.Artikel
            .AsNoTracking()
            .Include(a => a.Zutaten).ThenInclude(az => az.Zutat)
            .ToListAsync(cancellationToken);

    public Task<List<int>> GetExistingIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) =>
        dbContext.Artikel
            .Where(a => ids.Contains(a.Id))
            .Select(a => a.Id)
            .ToListAsync(cancellationToken);
}
