using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Application.Interfaces.Persistence;

public interface ILagerbestandRepository
{
    Task<List<Lagerbestand>> GetAllWithZutatAsync(CancellationToken cancellationToken = default);
    Task<Lagerbestand?> GetByZutatIdAsync(int zutatId, CancellationToken cancellationToken = default);
}
