namespace SmartRestaurant.Domain.Entities;

public sealed class ArtikelZutat
{
    public int ArtikelId { get; set; }
    public int ZutatId { get; set; }
    public int Menge { get; set; }
    public Artikel Artikel { get; set; } = null!;
    public Zutat Zutat { get; set; } = null!;
}
