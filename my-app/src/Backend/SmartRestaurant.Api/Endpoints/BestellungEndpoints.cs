using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Models;

namespace SmartRestaurant.Api.Endpoints;

public static class BestellungEndpoints
{
    public static void MapBestellungEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bestellungen", async (NeueBestellungRequest request, SmartRestaurantDbContext db) =>
        {
            if (request.Positionen.Count == 0)
            {
                return Results.BadRequest(new { message = "Eine Bestellung benötigt mindestens eine Position." });
            }

            var tischExistiert = await db.Tische.AnyAsync(t => t.Id == request.TischId);
            if (!tischExistiert)
            {
                return Results.NotFound(new { message = $"Tisch mit Id {request.TischId} wurde nicht gefunden." });
            }

            var artikelIds = request.Positionen.Select(p => p.ArtikelId).Distinct().ToList();
            var vorhandeneArtikelIds = await db.Artikel
                .Where(a => artikelIds.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync();

            var fehlendeArtikelIds = artikelIds.Except(vorhandeneArtikelIds).ToList();
            if (fehlendeArtikelIds.Count > 0)
            {
                return Results.NotFound(new { message = $"Artikel nicht gefunden: {string.Join(", ", fehlendeArtikelIds)}" });
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

            db.Bestellungen.Add(bestellung);
            await db.SaveChangesAsync();

            var response = await LadeBestellungResponse(db, bestellung.Id);
            return Results.Created($"/api/bestellungen/{bestellung.Id}", response);
        });
        
        app.MapGet("/api/bestellungen/{id:int}", async (int id, SmartRestaurantDbContext db) =>
        {
            var response = await LadeBestellungResponse(db, id);
            return response is null ? Results.NotFound() : Results.Ok(response);
        });
        
        app.MapPatch("/api/bestellungen/{id:int}/status", async (int id, BestellungStatusRequest request, SmartRestaurantDbContext db) =>
        {
            var bestellung = await db.Bestellungen.FirstOrDefaultAsync(b => b.Id == id);
            if (bestellung is null)
            {
                return Results.NotFound();
            }

            var mitarbeiterExistiert = await db.Mitarbeiter.AnyAsync(m => m.Id == request.MitarbeiterId);
            if (!mitarbeiterExistiert)
            {
                return Results.NotFound(new { message = $"Mitarbeiter mit Id {request.MitarbeiterId} wurde nicht gefunden." });
            }

            bestellung.Status = request.NeuerStatus;

            db.StatusLogs.Add(new StatusLog
            {
                BestellungId = bestellung.Id,
                MitarbeiterId = request.MitarbeiterId,
                Status = request.NeuerStatus,
                Zeitpunkt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            });

            await db.SaveChangesAsync();

            var response = await LadeBestellungResponse(db, bestellung.Id);
            return Results.Ok(response);
        });
    }

    private static async Task<BestellungResponse?> LadeBestellungResponse(SmartRestaurantDbContext db, int bestellungId)
    {
        var bestellung = await db.Bestellungen
            .Include(b => b.Positionen).ThenInclude(p => p.Artikel)
            .Include(b => b.StatusLogs).ThenInclude(l => l.Mitarbeiter)
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.Id == bestellungId);

        if (bestellung is null)
            return null;
        
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
