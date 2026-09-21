namespace SmartRestaurant.Api.Endpoints;

/// <summary>Einfaches Fehlerobjekt, das bei 400/404-Antworten mit Zusatzkontext zurückgegeben wird.</summary>
public sealed class ErrorResponse
{
    public required string Message { get; set; }
}
