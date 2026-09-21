using Moq;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Services;
using SmartRestaurant.Domain.Entities;
using Xunit;

namespace SmartRestaurant.Application.Tests.Services;

public class StammdatenServiceTests
{
    private readonly Mock<ITischRepository> _tischRepository = new();
    private readonly Mock<IMitarbeiterRepository> _mitarbeiterRepository = new();
    private readonly StammdatenService _sut;

    public StammdatenServiceTests()
    {
        _sut = new StammdatenService(_tischRepository.Object, _mitarbeiterRepository.Object);
    }

    [Fact]
    public async Task GetTischeAsync_MapptTischeAufResponse()
    {
        _tischRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new Tisch { Id = 1, Plaetze = 4, Status = true },
                new Tisch { Id = 2, Plaetze = 2, Status = false }
            ]);

        var result = await _sut.GetTischeAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].TischId);
        Assert.Equal(4, result[0].Plaetze);
        Assert.True(result[0].IstBelegt);
        Assert.False(result[1].IstBelegt);
    }

    [Fact]
    public async Task GetMitarbeiterAsync_MapptMitarbeiterAufResponse()
    {
        _mitarbeiterRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new Mitarbeiter { Id = 1, Name = "Max Mustermann", Benutzername = "max", Rolle = "Service" }
            ]);

        var result = await _sut.GetMitarbeiterAsync();

        var response = Assert.Single(result);
        Assert.Equal(1, response.MitarbeiterId);
        Assert.Equal("Max Mustermann", response.Name);
        Assert.Equal("max", response.Benutzername);
        Assert.Equal("Service", response.Rolle);
    }
}
