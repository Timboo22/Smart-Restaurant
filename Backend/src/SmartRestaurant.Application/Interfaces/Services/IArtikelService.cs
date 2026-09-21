using SmartRestaurant.Application.Dtos;

namespace SmartRestaurant.Application.Interfaces.Services;

public interface IArtikelService
{
    Task<List<ArtikelResponse>> GetArtikelAsync(CancellationToken cancellationToken = default);
}
