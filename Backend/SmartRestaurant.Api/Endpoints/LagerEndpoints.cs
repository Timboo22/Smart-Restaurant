using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Models;

namespace SmartRestaurant.Api.Endpoints;

public static class LagerEndpoints
{
    public static void MapLagerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/lager", async (SmartRestaurantDbContext db) =>
        {
            var lager = await db.Lagerbestaende
                .AsNoTracking()
                .Select(l => new LagerbestandResponse
                {
                    ZutatenId = l.ZutatId,
                    ZutatenName = l.Zutat.Name,
                    Soll = l.Soll,
                    Ist = l.Ist,
                    NachbestellenErforderlich = l.Ist < l.Soll
                })
                .ToListAsync();

            return Results.Ok(lager);
        });

        // PUT /api/lager/{zutatenId}
        app.MapPut("/api/lager/{zutatenId:int}", async (int zutatenId, LagerbestandUpdateRequest request, SmartRestaurantDbContext db) =>
        {
            var lagerbestand = await db.Lagerbestaende
                .Include(l => l.Zutat)
                .FirstOrDefaultAsync(l => l.ZutatId == zutatenId);

            if (lagerbestand is null)
            {
                return Results.NotFound();
            }

            lagerbestand.Soll = request.Soll;
            lagerbestand.Ist = request.Ist;
            await db.SaveChangesAsync();

            return Results.Ok(new LagerbestandResponse
            {
                ZutatenId = lagerbestand.ZutatId,
                ZutatenName = lagerbestand.Zutat.Name,
                Soll = lagerbestand.Soll,
                Ist = lagerbestand.Ist,
                NachbestellenErforderlich = lagerbestand.Ist < lagerbestand.Soll
            });
        });
    }
}
