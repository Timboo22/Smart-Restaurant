using Moq;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Services;
using SmartRestaurant.Domain.Entities;
using Xunit;

namespace SmartRestaurant.Application.Tests.Services;

public class ArtikelServiceTests
{
    private readonly Mock<IArtikelRepository> _artikelRepository = new();
    private readonly ArtikelService _sut;

    public ArtikelServiceTests()
    {
        _sut = new ArtikelService(_artikelRepository.Object);
    }

    [Fact]
    public async Task GetArtikelAsync_MapptArtikelUndZutatenAufResponse()
    {
        var zutat = new Zutat { Id = 1, Name = "Käse" };
        var artikel = new Artikel
        {
            Id = 1,
            Name = "Pizza Margherita",
            Preis = 9.5m,
            Kategorie = "Hauptgericht",
            Zutaten = [new ArtikelZutat { ArtikelId = 1, ZutatId = 1, Zutat = zutat, Menge = 2 }]
        };

        _artikelRepository
            .Setup(r => r.GetAllWithZutatenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([artikel]);

        var result = await _sut.GetArtikelAsync();

        var response = Assert.Single(result);
        Assert.Equal(1, response.ArtikelId);
        Assert.Equal("Pizza Margherita", response.Name);
        Assert.Equal(9.5m, response.Preis);

        var zutatResponse = Assert.Single(response.Zutaten);
        Assert.Equal(1, zutatResponse.ZutatenId);
        Assert.Equal("Käse", zutatResponse.ZutatenName);
        Assert.Equal(2, zutatResponse.Anzahl);
    }

    [Fact]
    public async Task GetArtikelAsync_GibtLeereListeZurueck_WennKeineArtikelVorhanden()
    {
        _artikelRepository
            .Setup(r => r.GetAllWithZutatenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _sut.GetArtikelAsync();

        Assert.Empty(result);
    }
}
