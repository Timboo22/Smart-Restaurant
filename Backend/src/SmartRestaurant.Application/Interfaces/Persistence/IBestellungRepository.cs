using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Application.Interfaces.Persistence;

public interface IBestellungRepository
{
    Task AddAsync(Bestellung bestellung, CancellationToken cancellationToken = default);
    Task<Bestellung?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Bestellung?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    void AddStatusLog(StatusLog statusLog);
}
