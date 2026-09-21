namespace SmartRestaurant.Application.Dtos;

public sealed class LagerbestandResponse
{
    public int ZutatenId { get; set; }
    public required string ZutatenName { get; set; }
    public int Soll { get; set; }
    public int Ist { get; set; }
    public bool NachbestellenErforderlich { get; set; }
}

public sealed class LagerbestandUpdateRequest
{
    public int Soll { get; set; }
    public int Ist { get; set; }
}
