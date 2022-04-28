using System.Collections.Generic;


namespace Domain.Entities;


public sealed class Floor
{
    public int Number { get; set; }

    public List<Room> Rooms { get; } = new List<Room>();
    public List<CleaningSchedule> Cleaners { get; } = new List<CleaningSchedule>();
}
