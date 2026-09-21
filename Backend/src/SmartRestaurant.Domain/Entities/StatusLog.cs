namespace SmartRestaurant.Domain.Entities;

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
