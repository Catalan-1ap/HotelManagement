using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Domain.Entities;


public sealed class Client
{
    public string? Passport { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateTime? Arrival { get; set; } = null!;
    public bool IsCheckout { get; set; } = true;

    public static Expression<Func<Client, bool>> CanCheckOut { get; } = c => c.IsCheckout == false;
    public static Expression<Func<Client, bool>> CanCheckIn { get; } = c => c.IsCheckout;

    public string? RoomId { get; set; }
    public Room? Room { get; set; }

    public ICollection<RoomReport> Visits { get; } = new HashSet<RoomReport>();
}
