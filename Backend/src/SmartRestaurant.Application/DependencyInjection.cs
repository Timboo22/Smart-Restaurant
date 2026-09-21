using Microsoft.Extensions.DependencyInjection;
using SmartRestaurant.Application.Interfaces.Services;
using SmartRestaurant.Application.Services;

namespace SmartRestaurant.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStammdatenService, StammdatenService>();
        services.AddScoped<IArtikelService, ArtikelService>();
        services.AddScoped<ILagerService, LagerService>();
        services.AddScoped<IBestellungService, BestellungService>();

        return services;
    }
}
