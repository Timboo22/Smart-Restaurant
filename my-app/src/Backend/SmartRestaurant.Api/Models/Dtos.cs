namespace SmartRestaurant.Api.Models;

// ---------- Bestellungen ----------

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

// ---------- Speisekarte & Artikel ----------

public sealed class ArtikelResponse
{
    public int ArtikelId { get; set; }
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required string Kategorie { get; set; }
    public List<ArtikelZutatResponse> Zutaten { get; set; } = [];
}

public sealed class ArtikelZutatResponse
{
    public int ZutatenId { get; set; }
    public required string ZutatenName { get; set; }
    public int Anzahl { get; set; }
}

// ---------- Lager & Zutatenbestand ----------

public sealed class LagerbestandResponse
{
    public int ZutatenId { get; set; }
    public required string ZutatenName { get; set; }
    public int Soll { get; set; }
    public int Ist { get; set; }
    public bool NachbestellenErforderlich { get; set; }
}

public sealed class LagerbestandUpdateRequest
{
    public int Soll { get; set; }
    public int Ist { get; set; }
}

// ---------- Tische & Mitarbeiter ----------

public sealed class TischResponse
{
    public int TischId { get; set; }
    public int Plaetze { get; set; }
    public bool IstBelegt { get; set; }
}

public sealed class MitarbeiterResponse
{
    public int MitarbeiterId { get; set; }
    public required string Name { get; set; }
    public required string Benutzername { get; set; }
    public required string Rolle { get; set; }
}
