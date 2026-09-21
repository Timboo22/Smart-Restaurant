using Scalar.AspNetCore;
using SmartRestaurant.Api.Endpoints;
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
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.CreateEndpoints();

app.Run();
