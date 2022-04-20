using System;
using Domain.Enums;


namespace Application.Exceptions;


public sealed class CleaningScheduleForCleanerAlreadyExistsException : Exception
{
    public CleaningScheduleForCleanerAlreadyExistsException(int cleanerId, Weekday weekday)
        : base(
            $"Cleaner with id \"{cleanerId}\" already have schedule in {weekday}")
    {
        CleanerId = cleanerId;
        Weekday = weekday;
    }


    public int CleanerId { get; }
    public Weekday Weekday { get; }
}
