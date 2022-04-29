using System;


namespace Domain.Entities;


public sealed class RoomReport
{
    public int Id { get; set; }
    public int DaysNumber { get; set; }
    public int TotalPrice { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Depart { get; set; }

    public string RoomId { get; set; } = string.Empty;
    public Room? Room { get; set; }

    public string ClientId { get; set; } = string.Empty;
    public Client? Client { get; set; }
}
