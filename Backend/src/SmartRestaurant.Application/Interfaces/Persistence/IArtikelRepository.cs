using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Application.Interfaces.Persistence;

public interface IArtikelRepository
{
    Task<List<Artikel>> GetAllWithZutatenAsync(CancellationToken cancellationToken = default);
    Task<List<int>> GetExistingIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
