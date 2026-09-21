namespace SmartRestaurant.Domain.Entities;

public sealed class Lagerbestand
{
    public int Id { get; set; }
    public int ZutatId { get; set; }
    public int Soll { get; set; }
    public int Ist { get; set; }
    public Zutat Zutat { get; set; } = null!;
}
