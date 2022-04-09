using System.Collections.Generic;


namespace Domain.Entities;


public sealed class Floor
{
    public int Number { get; set; }

    public ICollection<Room> Rooms { get; } = new HashSet<Room>();
    public ICollection<CleaningSchedule> Cleaners { get; } = new HashSet<CleaningSchedule>();
}
