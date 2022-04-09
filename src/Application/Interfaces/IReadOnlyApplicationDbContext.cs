using System.Linq;
using Domain.Entities;


namespace Application.Interfaces;


public interface IReadOnlyApplicationDbContext
{
    IQueryable<Room> Rooms { get; }
    IQueryable<RoomReport> RoomReports { get; }
    IQueryable<Person> Persons { get; }
    IQueryable<Client> Clients { get; }
    IQueryable<Cleaner> Cleaners { get; }
    IQueryable<Floor> Floors { get; }
    IQueryable<CleaningSchedule> CleaningSchedule { get; }
    IQueryable<RoomType> RoomTypes { get; }
}
