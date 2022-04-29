using System;
using Application.Interfaces;


namespace Infrastructure.Services;


internal class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
