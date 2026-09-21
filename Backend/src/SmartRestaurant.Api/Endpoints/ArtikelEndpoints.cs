using SmartRestaurant.Application.Dtos;
using SmartRestaurant.Application.Interfaces.Services;

namespace SmartRestaurant.Api.Endpoints;

public static class ArtikelEndpoints
{
    public static void MapArtikelEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/artikel", async (IArtikelService artikelService, CancellationToken cancellationToken) =>
        {
            var artikel = await artikelService.GetArtikelAsync(cancellationToken);
            return Results.Ok(artikel);
        })
        .WithName("GetArtikel")
        .WithTags("Artikel")
        .WithSummary("Speisekarte inkl. Zutaten abrufen")
        .WithDescription(
            "Liefert alle Artikel inklusive der für jeden Artikel benötigten Zutaten, z. B. für die Anzeige " +
            "in der Speisekarte oder eine Verfügbarkeitsprüfung im Frontend.")
        .Produces<List<ArtikelResponse>>(StatusCodes.Status200OK);
    }
}
