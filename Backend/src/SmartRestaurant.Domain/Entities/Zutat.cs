namespace SmartRestaurant.Domain.Entities;

public sealed class Zutat
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<ArtikelZutat> Artikel { get; set; } = [];
    public ICollection<Lagerbestand> Lagerbestaende { get; set; } = [];
}
