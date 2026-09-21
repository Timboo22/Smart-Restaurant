using SmartRestaurant.Application.Interfaces.Persistence;

namespace SmartRestaurant.Infrastructure.Persistence;

public sealed class UnitOfWork(SmartRestaurantDbContext dbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
