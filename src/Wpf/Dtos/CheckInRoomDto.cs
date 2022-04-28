namespace Wpf.Dtos;


public sealed class CheckInRoomDto
{
    public string? Number { get; init; }
    public int FloorNumber { get; init; }
    public int PricePerDay { get; init; }
    public string? Description { get; init; }
    public int TotalPlaces { get; init; }
    public int FreePlaces { get; init; }
}