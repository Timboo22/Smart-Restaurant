namespace SmartRestaurant.Domain.Entities;

public sealed class Tisch
{
    public int Id { get; set; }
    public int Plaetze { get; set; }
    public bool Status { get; set; }
    public ICollection<Bestellung> Bestellungen { get; set; } = [];
}
