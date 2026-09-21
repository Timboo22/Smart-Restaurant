namespace SmartRestaurant.Application.Dtos;

public sealed class NeueBestellungRequest
{
    public int TischId { get; set; }
    public List<NeueBestellpositionRequest> Positionen { get; set; } = [];
}

public sealed class NeueBestellpositionRequest
{
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
}

public sealed class BestellungStatusRequest
{
    public int MitarbeiterId { get; set; }
    public required string NeuerStatus { get; set; }
}

public sealed class BestellungResponse
{
    public int BestellungId { get; set; }
    public int TischId { get; set; }
    public required string Status { get; set; }
    public DateTime Zeitpunkt { get; set; }
    public decimal Gesamtbetrag { get; set; }
    public List<BestellpositionResponse> Positionen { get; set; } = [];
    public List<StatusLogResponse> StatusLogs { get; set; } = [];
}

public sealed class BestellpositionResponse
{
    public int BestellpositionId { get; set; }
    public int ArtikelId { get; set; }
    public required string ArtikelName { get; set; }
    public decimal Einzelpreis { get; set; }
    public int Menge { get; set; }
    public decimal Gesamtpreis { get; set; }
}

public sealed class StatusLogResponse
{
    public int LogId { get; set; }
    public int MitarbeiterId { get; set; }
    public required string MitarbeiterName { get; set; }
    public required string Status { get; set; }
    public DateTime Zeitpunkt { get; set; }
}
