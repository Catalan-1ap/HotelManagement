using System.Collections.Generic;


namespace Domain.Entities;


public sealed class Room
{
    public string? Number { get; set; }

    public int? RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    public int FloorId { get; set; }
    public Floor? Floor { get; set; }

    public List<Client> Clients { get; } = new List<Client>();
    public List<RoomReport> Reports { get; } = new List<RoomReport>();
}
