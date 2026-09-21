using Moq;
using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Services;
using SmartRestaurant.Domain.Entities;
using Xunit;

namespace SmartRestaurant.Application.Tests.Services;

public class LagerServiceTests
{
    private readonly Mock<ILagerbestandRepository> _lagerbestandRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly LagerService _sut;

    public LagerServiceTests()
    {
        _sut = new LagerService(_lagerbestandRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetLagerbestandAsync_MarkiertNachbestellenErforderlich_WennIstUnterSoll()
    {
        _lagerbestandRepository
            .Setup(r => r.GetAllWithZutatAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new Lagerbestand { Id = 1, ZutatId = 1, Zutat = new Zutat { Id = 1, Name = "Tomaten" }, Soll = 10, Ist = 4 },
                new Lagerbestand { Id = 2, ZutatId = 2, Zutat = new Zutat { Id = 2, Name = "Käse" }, Soll = 5, Ist = 5 }
            ]);

        var result = await _sut.GetLagerbestandAsync();

        Assert.Equal(2, result.Count);
        Assert.True(result[0].NachbestellenErforderlich);
        Assert.False(result[1].NachbestellenErforderlich);
    }

    [Fact]
    public async Task UpdateLagerbestandAsync_GibtNotFoundZurueck_WennZutatFehlt()
    {
        _lagerbestandRepository
            .Setup(r => r.GetByZutatIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Lagerbestand?)null);

        var result = await _sut.UpdateLagerbestandAsync(99, new LagerbestandUpdateRequest { Soll = 10, Ist = 5 });

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateLagerbestandAsync_AktualisiertBestandUndSpeichert()
    {
        var lagerbestand = new Lagerbestand
        {
            Id = 1,
            ZutatId = 1,
            Zutat = new Zutat { Id = 1, Name = "Tomaten" },
            Soll = 10,
            Ist = 2
        };

        _lagerbestandRepository
            .Setup(r => r.GetByZutatIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lagerbestand);

        var result = await _sut.UpdateLagerbestandAsync(1, new LagerbestandUpdateRequest { Soll = 20, Ist = 20 });

        Assert.True(result.IsSuccess);
        Assert.Equal(20, lagerbestand.Soll);
        Assert.Equal(20, lagerbestand.Ist);
        Assert.False(result.Value!.NachbestellenErforderlich);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
