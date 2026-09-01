using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApplication1.Data;
using WebApplication1.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<SmartRestaurantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartRestaurant")
        ?? throw new InvalidOperationException(
            "Connection string 'SmartRestaurant' was not configured.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.CreateEndpoints();

app.Run();
