using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Models;

namespace SmartRestaurant.Api.Endpoints;

public static class ArtikelEndpoints
{
    public static void MapArtikelEndpoints(this IEndpointRouteBuilder app)
    {
        // GET /api/artikel
        app.MapGet("/api/artikel", async (SmartRestaurantDbContext db) =>
        {
            var artikel = await db.Artikel
                .AsNoTracking()
                .Select(a => new ArtikelResponse
                {
                    ArtikelId = a.Id,
                    Name = a.Name,
                    Preis = a.Preis,
                    Kategorie = a.Kategorie,
                    Zutaten = a.Zutaten.Select(az => new ArtikelZutatResponse
                    {
                        ZutatenId = az.ZutatId,
                        ZutatenName = az.Zutat.Name,
                        Anzahl = az.Menge
                    }).ToList()
                })
                .ToListAsync();

            return Results.Ok(artikel);
        });
    }
}
