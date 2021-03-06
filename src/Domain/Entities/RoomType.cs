using System.Collections.Generic;


namespace Domain.Entities;


public sealed class RoomType
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int MaxPeopleNumber { get; set; }
    public int PricePerDay { get; set; }

    public List<Room> Rooms { get; } = new List<Room>();
}
