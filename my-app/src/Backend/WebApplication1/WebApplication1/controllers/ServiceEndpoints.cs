namespace WebApplication1.Endpoints;

public static class ServiceEndpoints
{
    public static void CreateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/artikel", () => Results.Ok("Artikel Liste"));
    }
}
