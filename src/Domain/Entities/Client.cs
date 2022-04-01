using System;
using System.Collections.Generic;


namespace Domain.Entities;


public sealed class Client
{
    public string Passport { get; set; } = null!;
    public string? City { get; set; }
    public DateTime Arrival { get; set; } = DateTime.UtcNow;
    public bool IsCheckout { get; set; } = true;

    public string? RoomId { get; set; }
    public Room? Room { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }

    public ICollection<RoomReport> Visits { get; } = new HashSet<RoomReport>();
}
