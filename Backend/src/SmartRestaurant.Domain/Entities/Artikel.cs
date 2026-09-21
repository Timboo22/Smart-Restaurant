namespace SmartRestaurant.Domain.Entities;

public sealed class Artikel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Preis { get; set; }
    public required string Kategorie { get; set; }
    public ICollection<Bestellposition> Bestellpositionen { get; set; } = [];
    public ICollection<ArtikelZutat> Zutaten { get; set; } = [];
}
