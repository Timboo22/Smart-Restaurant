using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using SmartRestaurant.Api.Endpoints;
using SmartRestaurant.Api.Middleware;
using SmartRestaurant.Application;
using SmartRestaurant.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Smart Restaurant API";
        document.Info.Version = "v1";
        document.Info.Description =
            "API für Bestellungen, Speisekarte, Lagerbestand sowie Tisch- und Mitarbeiter-Stammdaten " +
            "des Smart Restaurant Systems. Alle Felder sind camelCase.";
        return Task.CompletedTask;
    });
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description
            }),
            durationMs = report.TotalDuration.TotalMilliseconds
        };

        await context.Response.WriteAsJsonAsync(payload);
    }
})
.WithName("GetHealth")
.WithTags("Health")
.WithSummary("Health-Status der API und der Datenbankverbindung");

app.CreateEndpoints();

app.Run();
