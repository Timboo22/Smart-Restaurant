namespace SmartRestaurant.Api.Models;

public sealed class Mitarbeiter
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Benutzername { get; set; }
    public required string Rolle { get; set; }
    public ICollection<StatusLog> StatusLogs { get; set; } = [];
}

public sealed class Tisch
{
    public int Id { get; set; }
    public int Plaetze { get; set; }
    public bool Status { get; set; }
    public ICollection<Bestellung> Bestellungen { get; set; } = [];
}

public sealed class Artikel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required string Kategorie { get; set; }
    public ICollection<Bestellposition> Bestellpositionen { get; set; } = [];
    public ICollection<ArtikelZutat> Zutaten { get; set; } = [];
}

public sealed class Zutat
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<ArtikelZutat> Artikel { get; set; } = [];
    public ICollection<Lagerbestand> Lagerbestaende { get; set; } = [];
}

public sealed class Bestellung
{
    public int Id { get; set; }
    public int TischId { get; set; }
    public required string Status { get; set; }
    public DateTime Zeitpunkt { get; set; }
    public Tisch Tisch { get; set; } = null!;
    public ICollection<StatusLog> StatusLogs { get; set; } = [];
    public ICollection<Bestellposition> Positionen { get; set; } = [];
}

public sealed class StatusLog
{
    public int Id { get; set; }
    public int BestellungId { get; set; }
    public int MitarbeiterId { get; set; }
    public required string Status { get; set; }
    public DateTime Zeitpunkt { get; set; }
    public Bestellung Bestellung { get; set; } = null!;
    public Mitarbeiter Mitarbeiter { get; set; } = null!;
}

public sealed class Bestellposition
{
    public int Id { get; set; }
    public int BestellungId { get; set; }
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public Bestellung Bestellung { get; set; } = null!;
    public Artikel Artikel { get; set; } = null!;
}

public sealed class ArtikelZutat
{
    public int ArtikelId { get; set; }
    public int ZutatId { get; set; }
    public int Menge { get; set; }
    public Artikel Artikel { get; set; } = null!;
    public Zutat Zutat { get; set; } = null!;
}

public sealed class Lagerbestand
{
    public int Id { get; set; }
    public int ZutatId { get; set; }
    public int Soll { get; set; }
    public int Ist { get; set; }
    public Zutat Zutat { get; set; } = null!;
}
