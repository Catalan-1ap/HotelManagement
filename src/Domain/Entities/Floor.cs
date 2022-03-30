using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Floor : IBaseEntity
{
    public int Id { get; set; }
    public int Number { get; set; }

    public ICollection<Room> Rooms { get; } = new HashSet<Room>();
    public ICollection<FloorCleaner> Cleaners { get; } = new HashSet<FloorCleaner>();
}
