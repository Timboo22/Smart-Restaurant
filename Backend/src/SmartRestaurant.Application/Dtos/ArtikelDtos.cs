namespace SmartRestaurant.Application.Dtos;

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
