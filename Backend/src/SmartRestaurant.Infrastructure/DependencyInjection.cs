using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartRestaurant.Application.Interfaces.Persistence;
using SmartRestaurant.Infrastructure.Persistence;
using SmartRestaurant.Infrastructure.Repositories;

namespace SmartRestaurant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SmartRestaurantDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SmartRestaurant")
                ?? throw new InvalidOperationException(
                    "Connection string 'SmartRestaurant' was not configured.")));

        services.AddHealthChecks()
            .AddDbContextCheck<SmartRestaurantDbContext>("database");

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITischRepository, TischRepository>();
        services.AddScoped<IMitarbeiterRepository, MitarbeiterRepository>();
        services.AddScoped<IArtikelRepository, ArtikelRepository>();
        services.AddScoped<ILagerbestandRepository, LagerbestandRepository>();
        services.AddScoped<IBestellungRepository, BestellungRepository>();

        return services;
    }
}
