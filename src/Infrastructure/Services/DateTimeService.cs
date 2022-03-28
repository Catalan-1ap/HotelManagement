using System;
using Application.Interfaces;


namespace Infrastructure.Services;



internal class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}

