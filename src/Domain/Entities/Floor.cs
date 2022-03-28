using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Floor : IBaseEntity
{
    public int Id { get; set; }
    public int Number { get; set; }

    public ICollection<Room> Rooms { get; } = null!;
    public ICollection<FloorCleaner> Cleaners { get; } = null!;
}
