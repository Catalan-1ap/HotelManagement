using Domain.Enums;


namespace Domain.Entities;


public sealed class CleaningSchedule
{
    public Weekday Weekday { get; set; }

    public int FloorId { get; set; }
    public Floor? Floor { get; set; }

    public int CleanerId { get; set; }
    public Cleaner? Cleaner { get; set; }
}
