using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Api.Endpoints;

public static class BestellungEndpoints
{
    public static void MapBestellungEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bestellungen", async (
            NeueBestellungRequest request,
            IBestellungService bestellungService,
            CancellationToken cancellationToken) =>
        {
            var result = await bestellungService.CreateBestellungAsync(request, cancellationToken);
            return result.ToHttpResult(response => Results.Created($"/api/bestellungen/{response.BestellungId}", response));
        })
        .WithName("CreateBestellung")
        .WithTags("Bestellungen")
        .WithSummary("Neue Bestellung aufgeben")
        .WithDescription(
            "Legt eine neue Bestellung für einen Tisch an. Der Status wird automatisch auf \"Aufgenommen\" " +
            "gesetzt, der Zeitpunkt wird serverseitig erzeugt. Erfordert mindestens eine Position sowie eine " +
            "existierende Tisch- und Artikel-Id je Position.")
        .Produces<BestellungResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        app.MapGet("/api/bestellungen/{id:int}", async (
            int id,
            IBestellungService bestellungService,
            CancellationToken cancellationToken) =>
        {
            var response = await bestellungService.GetBestellungAsync(id, cancellationToken);
            return response is null ? Results.NotFound() : Results.Ok(response);
        })
        .WithName("GetBestellung")
        .WithTags("Bestellungen")
        .WithSummary("Bestellung Details abrufen")
        .WithDescription(
            "Liefert eine einzelne Bestellung inklusive aller Positionen (mit aktuellem Artikelnamen/-preis) " +
            "und der vollständigen, chronologisch aufsteigend sortierten Status-Historie.")
        .Produces<BestellungResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPatch("/api/bestellungen/{id:int}/status", async (
            int id,
            BestellungStatusRequest request,
            IBestellungService bestellungService,
            CancellationToken cancellationToken) =>
        {
            var result = await bestellungService.UpdateStatusAsync(id, request, cancellationToken);
            return result.ToHttpResult(Results.Ok);
        })
        .WithName("UpdateBestellungStatus")
        .WithTags("Bestellungen")
        .WithSummary("Bestellstatus ändern")
        .WithDescription(
            "Setzt den Status der Bestellung neu und erzeugt automatisch einen Eintrag in der Status-Historie. " +
            "\"neuerStatus\" ist ein freies Textfeld (kein serverseitiges Enum) – für funktionierende Status-Filter " +
            "im Frontend immer dieselben Strings verwenden, z. B. \"Aufgenommen\", \"In Zubereitung\", " +
            "\"Servierbereit\", \"Bezahlt\".")
        .Produces<BestellungResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
    }
}
