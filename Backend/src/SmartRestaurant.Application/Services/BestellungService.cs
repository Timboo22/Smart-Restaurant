using SmartRestaurant.Application.Common;
using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Application.Interfaces.Services;
using SmartRestaurant.Domain.Entities;

namespace SmartRestaurant.Application.Services;

public sealed class BestellungService(
    IBestellungRepository bestellungRepository,
    ITischRepository tischRepository,
    IArtikelRepository artikelRepository,
    IMitarbeiterRepository mitarbeiterRepository,
    IUnitOfWork unitOfWork) : IBestellungService
{
    public async Task<Result<BestellungResponse>> CreateBestellungAsync(
        NeueBestellungRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Positionen.Count == 0)
        {
            return Result<BestellungResponse>.Failure(
                ErrorType.Validation,
                "Eine Bestellung benötigt mindestens eine Position.");
        }

        var tischExistiert = await tischRepository.ExistsAsync(request.TischId, cancellationToken);
        if (!tischExistiert)
        {
            return Result<BestellungResponse>.Failure(
                ErrorType.NotFound,
                $"Tisch mit Id {request.TischId} wurde nicht gefunden.");
        }

        var artikelIds = request.Positionen.Select(p => p.ArtikelId).Distinct().ToList();
        var vorhandeneArtikelIds = await artikelRepository.GetExistingIdsAsync(artikelIds, cancellationToken);

        var fehlendeArtikelIds = artikelIds.Except(vorhandeneArtikelIds).ToList();
        if (fehlendeArtikelIds.Count > 0)
        {
            return Result<BestellungResponse>.Failure(
                ErrorType.NotFound,
                $"Artikel nicht gefunden: {string.Join(", ", fehlendeArtikelIds)}");
        }

        var bestellung = new Bestellung
        {
            TischId = request.TischId,
            Status = "Aufgenommen",
            Zeitpunkt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
            Positionen = request.Positionen.Select(p => new Bestellposition
            {
                ArtikelId = p.ArtikelId,
                Menge = p.Menge
            }).ToList()
        };

        await bestellungRepository.AddAsync(bestellung, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = await LadeBestellungResponseAsync(bestellung.Id, cancellationToken);
        return Result<BestellungResponse>.Success(response!);
    }

    public Task<BestellungResponse?> GetBestellungAsync(int id, CancellationToken cancellationToken = default) =>
        LadeBestellungResponseAsync(id, cancellationToken);

    public async Task<Result<BestellungResponse>> UpdateStatusAsync(
        int id,
        BestellungStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var bestellung = await bestellungRepository.GetByIdAsync(id, cancellationToken);
        if (bestellung is null)
        {
            return Result<BestellungResponse>.Failure(
                ErrorType.NotFound,
                $"Bestellung mit Id {id} wurde nicht gefunden.");
        }

        var mitarbeiterExistiert = await mitarbeiterRepository.ExistsAsync(request.MitarbeiterId, cancellationToken);
        if (!mitarbeiterExistiert)
        {
            return Result<BestellungResponse>.Failure(
                ErrorType.NotFound,
                $"Mitarbeiter mit Id {request.MitarbeiterId} wurde nicht gefunden.");
        }

        bestellung.Status = request.NeuerStatus;

        bestellungRepository.AddStatusLog(new StatusLog
        {
            BestellungId = bestellung.Id,
            MitarbeiterId = request.MitarbeiterId,
            Status = request.NeuerStatus,
            Zeitpunkt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = await LadeBestellungResponseAsync(bestellung.Id, cancellationToken);
        return Result<BestellungResponse>.Success(response!);
    }

    private async Task<BestellungResponse?> LadeBestellungResponseAsync(int bestellungId, CancellationToken cancellationToken)
    {
        var bestellung = await bestellungRepository.GetByIdWithDetailsAsync(bestellungId, cancellationToken);
        if (bestellung is null)
        {
            return null;
        }

        var positionen = bestellung.Positionen.Select(p => new BestellpositionResponse
        {
            BestellpositionId = p.Id,
            ArtikelId = p.ArtikelId,
            ArtikelName = p.Artikel.Name,
            Einzelpreis = p.Artikel.Preis,
            Menge = p.Menge,
            Gesamtpreis = p.Artikel.Preis * p.Menge
        }).ToList();

        return new BestellungResponse
        {
            BestellungId = bestellung.Id,
            TischId = bestellung.TischId,
            Status = bestellung.Status,
            Zeitpunkt = bestellung.Zeitpunkt,
            Gesamtbetrag = positionen.Sum(p => p.Gesamtpreis),
            Positionen = positionen,
            StatusLogs = bestellung.StatusLogs
                .OrderBy(l => l.Zeitpunkt)
                .Select(l => new StatusLogResponse
                {
                    LogId = l.Id,
                    MitarbeiterId = l.MitarbeiterId,
                    MitarbeiterName = l.Mitarbeiter.Name,
                    Status = l.Status,
                    Zeitpunkt = l.Zeitpunkt
                }).ToList()
        };
    }
}
