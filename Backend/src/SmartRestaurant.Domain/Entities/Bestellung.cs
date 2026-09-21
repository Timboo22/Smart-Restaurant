namespace SmartRestaurant.Domain.Entities;

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
