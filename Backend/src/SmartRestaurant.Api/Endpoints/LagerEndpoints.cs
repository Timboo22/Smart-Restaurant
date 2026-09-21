using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Api.Endpoints;

public static class LagerEndpoints
{
    public static void MapLagerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/lager", async (ILagerService lagerService, CancellationToken cancellationToken) =>
        {
            var lager = await lagerService.GetLagerbestandAsync(cancellationToken);
            return Results.Ok(lager);
        })
        .WithName("GetLagerbestand")
        .WithTags("Lager")
        .WithSummary("Lagerbestand abfragen")
        .WithDescription(
            "Liefert den Lagerbestand je Zutat. \"nachbestellenErforderlich\" wird serverseitig berechnet " +
            "(true, wenn ist < soll) und sollte im Frontend direkt für eine Warnanzeige genutzt werden.")
        .Produces<List<LagerbestandResponse>>(StatusCodes.Status200OK);

        app.MapPut("/api/lager/{zutatenId:int}", async (
            int zutatenId,
            LagerbestandUpdateRequest request,
            ILagerService lagerService,
            CancellationToken cancellationToken) =>
        {
            var result = await lagerService.UpdateLagerbestandAsync(zutatenId, request, cancellationToken);
            return result.ToHttpResult(Results.Ok);
        })
        .WithName("UpdateLagerbestand")
        .WithTags("Lager")
        .WithSummary("Lagerbestand anpassen")
        .WithDescription(
            "Aktualisiert Soll- und Ist-Menge für eine Zutat. \"zutatenId\" ist die Id der Zutat " +
            "(zutatenId aus GET /api/lager bzw. GET /api/artikel), nicht eine interne Lagerbestand-Id.")
        .Produces<LagerbestandResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
    }
}
