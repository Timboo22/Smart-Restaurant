namespace SmartRestaurant.Api.Endpoints;

public static class ServiceEndpoints
{
    public static void CreateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapBestellungEndpoints();
        app.MapArtikelEndpoints();
        app.MapLagerEndpoints();
        app.MapStammdatenEndpoints();
    }
}