using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Application.Interfaces.Persistence;

public interface IMitarbeiterRepository
{
    Task<List<Mitarbeiter>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
