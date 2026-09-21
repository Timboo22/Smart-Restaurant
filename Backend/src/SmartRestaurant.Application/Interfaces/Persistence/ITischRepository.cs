using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Application.Interfaces.Persistence;

public interface ITischRepository
{
    Task<List<Tisch>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
