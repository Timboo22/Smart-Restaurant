namespace SmartRestaurant.Application.Dtos;

public sealed class TischResponse
{
    public int TischId { get; set; }
    public int Plaetze { get; set; }
    public bool IstBelegt { get; set; }
}

public sealed class TischStatusUpdateRequest
{
    public bool IstBelegt { get; set; }
}

public sealed class MitarbeiterResponse
{
    public int MitarbeiterId { get; set; }
    public required string Name { get; set; }
    public required string Benutzername { get; set; }
    public required string Rolle { get; set; }
}
