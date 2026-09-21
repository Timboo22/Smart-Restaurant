namespace SmartRestaurant.Domain.Entities;

public sealed class Bestellposition
{
    public int Id { get; set; }
    public int BestellungId { get; set; }
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
    public Bestellung Bestellung { get; set; } = null!;
    public Artikel Artikel { get; set; } = null!;
}
