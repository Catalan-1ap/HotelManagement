using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class RoomType : IBaseEntity
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int MaxPeopleNumber { get; set; }
    public int PricePerDay { get; set; }

    public ICollection<Room> Rooms { get; } = new HashSet<Room>();
}
