using System;
using System.Collections.Generic;
using Domain.Common;


namespace Domain.Entities;


public sealed class Client : IBaseEntity
{
    public int Id { get; set; }
    public string Passport { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime Arrival { get; set; }
    public bool IsCheckout { get; set; }

    public int? RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public ICollection<RoomReport> Visits { get; } = null!;
}
