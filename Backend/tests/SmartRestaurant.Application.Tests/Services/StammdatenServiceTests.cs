using Moq;
using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Services;
using SmartRestaurant.Domain.Entities;
using Xunit;

namespace SmartRestaurant.Application.Tests.Services;

public class StammdatenServiceTests
{
    private readonly Mock<ITischRepository> _tischRepository = new();
    private readonly Mock<IMitarbeiterRepository> _mitarbeiterRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly StammdatenService _sut;

    public StammdatenServiceTests()
    {
        _sut = new StammdatenService(_tischRepository.Object, _mitarbeiterRepository.Object, _unitOfWork.Object);
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

    [Fact]
    public async Task UpdateTischStatusAsync_GibtNotFoundZurueck_WennTischFehlt()
    {
        _tischRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tisch?)null);

        var result = await _sut.UpdateTischStatusAsync(99, new TischStatusUpdateRequest { IstBelegt = true });

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateTischStatusAsync_AktualisiertStatusUndSpeichert()
    {
        var tisch = new Tisch { Id = 1, Plaetze = 4, Status = false };
        _tischRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tisch);

        var result = await _sut.UpdateTischStatusAsync(1, new TischStatusUpdateRequest { IstBelegt = true });

        Assert.True(result.IsSuccess);
        Assert.True(tisch.Status);
        Assert.True(result.Value!.IstBelegt);
        Assert.Equal(1, result.Value!.TischId);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
