using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Application.Services;

public sealed class ArtikelService(IArtikelRepository artikelRepository) : IArtikelService
{
    public async Task<List<ArtikelResponse>> GetArtikelAsync(CancellationToken cancellationToken = default)
    {
        var artikel = await artikelRepository.GetAllWithZutatenAsync(cancellationToken);

        return artikel.Select(a => new ArtikelResponse
        {
            ArtikelId = a.Id,
            Name = a.Name,
            Preis = a.Preis,
            Kategorie = a.Kategorie,
            Zutaten = a.Zutaten.Select(az => new ArtikelZutatResponse
            {
                ZutatenId = az.ZutatId,
                ZutatenName = az.Zutat.Name,
                Anzahl = az.Menge
            }).ToList()
        }).ToList();
    }
}
