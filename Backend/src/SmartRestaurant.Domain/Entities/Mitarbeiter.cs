namespace SmartRestaurant.Domain.Entities;

public sealed class Mitarbeiter
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Benutzername { get; set; }
    public required string Rolle { get; set; }
    public ICollection<StatusLog> StatusLogs { get; set; } = [];
}
