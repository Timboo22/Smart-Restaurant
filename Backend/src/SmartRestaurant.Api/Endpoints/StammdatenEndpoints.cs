using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Api.Endpoints;

public static class StammdatenEndpoints
{
    public static void MapStammdatenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tische", async (IStammdatenService stammdatenService, CancellationToken cancellationToken) =>
        {
            var tische = await stammdatenService.GetTischeAsync(cancellationToken);
            return Results.Ok(tische);
        })
        .WithName("GetTische")
        .WithTags("Stammdaten")
        .WithSummary("Tischübersicht abrufen")
        .WithDescription("Liefert alle Tische mit Platzanzahl und Belegungsstatus.")
        .Produces<List<TischResponse>>(StatusCodes.Status200OK);

        app.MapGet("/api/mitarbeiter", async (IStammdatenService stammdatenService, CancellationToken cancellationToken) =>
        {
            var mitarbeiter = await stammdatenService.GetMitarbeiterAsync(cancellationToken);
            return Results.Ok(mitarbeiter);
        })
        .WithName("GetMitarbeiter")
        .WithTags("Stammdaten")
        .WithSummary("Mitarbeiterliste abrufen")
        .WithDescription("Liefert alle Mitarbeiter mit Name, Benutzername und Rolle.")
        .Produces<List<MitarbeiterResponse>>(StatusCodes.Status200OK);
    }
}
