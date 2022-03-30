using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Room : IBaseEntity
{
    public int Id { get; set; }
    public string? Number { get; set; }

    public int? RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    public int FloorId { get; set; }
    public Floor? Floor { get; set; }

    public ICollection<Client> Clients { get; } = new HashSet<Client>();
    public ICollection<RoomReport> Reports { get; } = new HashSet<RoomReport>();
}
