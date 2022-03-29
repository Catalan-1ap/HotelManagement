using Domain.Common;


namespace Domain.Entities;


public sealed class RoomReport : IBaseEntity
{
    public int Id { get; set; }
    public int DaysNumber { get; set; }
    public int TotalPrice { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; }
}
