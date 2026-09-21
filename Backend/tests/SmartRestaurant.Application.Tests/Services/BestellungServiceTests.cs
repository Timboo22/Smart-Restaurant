using Moq;
using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Services;
using SmartRestaurant.Domain.Entities;
using Xunit;

namespace SmartRestaurant.Application.Tests.Services;

public class BestellungServiceTests
{
    private readonly Mock<IBestellungRepository> _bestellungRepository = new();
    private readonly Mock<ITischRepository> _tischRepository = new();
    private readonly Mock<IArtikelRepository> _artikelRepository = new();
    private readonly Mock<IMitarbeiterRepository> _mitarbeiterRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly BestellungService _sut;

    public BestellungServiceTests()
    {
        _sut = new BestellungService(
            _bestellungRepository.Object,
            _tischRepository.Object,
            _artikelRepository.Object,
            _mitarbeiterRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task CreateBestellungAsync_GibtValidierungsfehlerZurueck_WennKeinePositionenVorhanden()
    {
        var request = new NeueBestellungRequest { TischId = 1, Positionen = [] };

        var result = await _sut.CreateBestellungAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.ErrorType);
        _tischRepository.Verify(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateBestellungAsync_GibtNotFoundZurueck_WennTischNichtExistiert()
    {
        var request = new NeueBestellungRequest
        {
            TischId = 5,
            Positionen = [new NeueBestellpositionRequest { ArtikelId = 1, Menge = 2 }]
        };
        _tischRepository.Setup(r => r.ExistsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _sut.CreateBestellungAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        Assert.Contains("Tisch", result.Error);
    }

    [Fact]
    public async Task CreateBestellungAsync_GibtNotFoundZurueck_WennArtikelFehlen()
    {
        var request = new NeueBestellungRequest
        {
            TischId = 1,
            Positionen =
            [
                new NeueBestellpositionRequest { ArtikelId = 1, Menge = 1 },
                new NeueBestellpositionRequest { ArtikelId = 2, Menge = 1 }
            ]
        };
        _tischRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _artikelRepository
            .Setup(r => r.GetExistingIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([1]);

        var result = await _sut.CreateBestellungAsync(request);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
        Assert.Contains("2", result.Error);
    }

    [Fact]
    public async Task CreateBestellungAsync_ErstelltBestellung_UndBerechnetGesamtbetrag()
    {
        var request = new NeueBestellungRequest
        {
            TischId = 1,
            Positionen =
            [
                new NeueBestellpositionRequest { ArtikelId = 10, Menge = 2 },
                new NeueBestellpositionRequest { ArtikelId = 20, Menge = 1 }
            ]
        };

        _tischRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _artikelRepository
            .Setup(r => r.GetExistingIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([10, 20]);

        Bestellung? angelegteBestellung = null;
        _bestellungRepository
            .Setup(r => r.AddAsync(It.IsAny<Bestellung>(), It.IsAny<CancellationToken>()))
            .Callback<Bestellung, CancellationToken>((b, _) =>
            {
                b.Id = 42;
                angelegteBestellung = b;
            })
            .Returns(Task.CompletedTask);

        var artikel10 = new Artikel { Id = 10, Name = "Pizza", Preis = 8.5m, Kategorie = "Hauptgericht" };
        var artikel20 = new Artikel { Id = 20, Name = "Cola", Preis = 3m, Kategorie = "Getränk" };

        _bestellungRepository
            .Setup(r => r.GetByIdWithDetailsAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new Bestellung
            {
                Id = 42,
                TischId = 1,
                Status = "Aufgenommen",
                Zeitpunkt = angelegteBestellung!.Zeitpunkt,
                Positionen =
                [
                    new Bestellposition { Id = 1, ArtikelId = 10, Artikel = artikel10, Menge = 2 },
                    new Bestellposition { Id = 2, ArtikelId = 20, Artikel = artikel20, Menge = 1 }
                ],
                StatusLogs = []
            });

        var result = await _sut.CreateBestellungAsync(request);

        Assert.True(result.IsSuccess);
        var response = result.Value!;
        Assert.Equal(42, response.BestellungId);
        Assert.Equal("Aufgenommen", response.Status);
        Assert.Equal(20m, response.Gesamtbetrag);
        Assert.Equal(2, response.Positionen.Count);
        Assert.Empty(response.StatusLogs);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBestellungAsync_GibtNullZurueck_WennNichtGefunden()
    {
        _bestellungRepository
            .Setup(r => r.GetByIdWithDetailsAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bestellung?)null);

        var result = await _sut.GetBestellungAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStatusAsync_GibtNotFoundZurueck_WennBestellungNichtExistiert()
    {
        _bestellungRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bestellung?)null);

        var result = await _sut.UpdateStatusAsync(1, new BestellungStatusRequest { MitarbeiterId = 1, NeuerStatus = "Fertig" });

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
    }

    [Fact]
    public async Task UpdateStatusAsync_GibtNotFoundZurueck_WennMitarbeiterNichtExistiert()
    {
        var bestellung = new Bestellung { Id = 1, TischId = 1, Status = "Aufgenommen", Zeitpunkt = DateTime.UtcNow };
        _bestellungRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(bestellung);
        _mitarbeiterRepository.Setup(r => r.ExistsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _sut.UpdateStatusAsync(1, new BestellungStatusRequest { MitarbeiterId = 5, NeuerStatus = "Fertig" });

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.ErrorType);
    }

    [Fact]
    public async Task UpdateStatusAsync_AktualisiertStatus_UndFuegtStatusLogHinzu()
    {
        var bestellung = new Bestellung { Id = 1, TischId = 1, Status = "Aufgenommen", Zeitpunkt = DateTime.UtcNow };
        _bestellungRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(bestellung);
        _mitarbeiterRepository.Setup(r => r.ExistsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        StatusLog? hinzugefuegterLog = null;
        _bestellungRepository
            .Setup(r => r.AddStatusLog(It.IsAny<StatusLog>()))
            .Callback<StatusLog>(log => hinzugefuegterLog = log);

        var mitarbeiter = new Mitarbeiter { Id = 5, Name = "Max Mustermann", Benutzername = "max", Rolle = "Service" };

        _bestellungRepository
            .Setup(r => r.GetByIdWithDetailsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new Bestellung
            {
                Id = 1,
                TischId = 1,
                Status = "Fertig",
                Zeitpunkt = bestellung.Zeitpunkt,
                Positionen = [],
                StatusLogs =
                [
                    new StatusLog
                    {
                        Id = 1,
                        BestellungId = 1,
                        MitarbeiterId = 5,
                        Mitarbeiter = mitarbeiter,
                        Status = "Fertig",
                        Zeitpunkt = hinzugefuegterLog!.Zeitpunkt
                    }
                ]
            });

        var result = await _sut.UpdateStatusAsync(1, new BestellungStatusRequest { MitarbeiterId = 5, NeuerStatus = "Fertig" });

        Assert.True(result.IsSuccess);
        Assert.Equal("Fertig", bestellung.Status);
        Assert.NotNull(hinzugefuegterLog);
        Assert.Equal(5, hinzugefuegterLog!.MitarbeiterId);
        Assert.Equal("Fertig", hinzugefuegterLog.Status);
        Assert.Single(result.Value!.StatusLogs);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
