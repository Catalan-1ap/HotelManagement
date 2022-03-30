using System;
using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Client : IBaseEntity
{
    public int Id { get; set; }
    public string? Passport { get; set; }
    public string? City { get; set; }
    public DateTime Arrival { get; set; }
    public bool IsCheckout { get; set; }

    public int? RoomId { get; set; }
    public Room? Room { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }

    public ICollection<RoomReport> Visits { get; } = new HashSet<RoomReport>();
}
