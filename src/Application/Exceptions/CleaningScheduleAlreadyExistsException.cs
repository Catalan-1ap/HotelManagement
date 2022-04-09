using System;
using Domain.Enums;


namespace Application.Exceptions;


public sealed class CleaningScheduleAlreadyExistsException : Exception
{
    public CleaningScheduleAlreadyExistsException(int floorNumber, int cleanerId, Weekday weekday)
        : base(
            $"Floor with number \"{floorNumber}\" already have schedule in {weekday} for cleaner with id {cleanerId}")
    {
        FloorNumber = floorNumber;
        CleanerId = cleanerId;
        Weekday = weekday;
    }


    public int FloorNumber { get; }
    public int CleanerId { get; }
    public Weekday Weekday { get; }
}
