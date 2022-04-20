using System;
using Domain.Enums;


namespace Application.Exceptions;


public sealed class CleaningScheduleForFloorAlreadyExistsException : Exception
{
    public CleaningScheduleForFloorAlreadyExistsException(int floorNumber, Weekday weekday)
        : base(
            $"Floor with number \"{floorNumber}\" already have schedule in {weekday}")
    {
        FloorNumber = floorNumber;
        Weekday = weekday;
    }


    public int FloorNumber { get; }
    public Weekday Weekday { get; }
}
