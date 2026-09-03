using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Models;

namespace SmartRestaurant.Api.Endpoints;

public static class StammdatenEndpoints
{
    public static void MapStammdatenEndpoints(this IEndpointRouteBuilder app)
    {
        // GET /api/tische
        app.MapGet("/api/tische", async (SmartRestaurantDbContext db) =>
        {
            var tische = await db.Tische
                .AsNoTracking()
                .Select(t => new TischResponse
                {
                    TischId = t.Id,
                    Plaetze = t.Plaetze,
                    IstBelegt = t.Status
                })
                .ToListAsync();

            return Results.Ok(tische);
        });

        // GET /api/mitarbeiter
        app.MapGet("/api/mitarbeiter", async (SmartRestaurantDbContext db) =>
        {
            var mitarbeiter = await db.Mitarbeiter
                .AsNoTracking()
                .Select(m => new MitarbeiterResponse
                {
                    MitarbeiterId = m.Id,
                    Name = m.Name,
                    Benutzername = m.Benutzername,
                    Rolle = m.Rolle
                })
                .ToListAsync();

            return Results.Ok(mitarbeiter);
        });
    }
}
