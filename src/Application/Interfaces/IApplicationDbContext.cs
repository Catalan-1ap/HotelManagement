using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Interfaces;


public interface IApplicationDbContext
{
    DbSet<Room> Rooms { get; set; }
    DbSet<RoomReport> RoomReports { get; set; }
    DbSet<Person> Persons { get; set; }
    DbSet<Client> Clients { get; set; }
    DbSet<Cleaner> Cleaners { get; set; }
    DbSet<Floor> Floors { get; set; }
    DbSet<FloorCleaner> FloorCleaners { get; set; }
    DbSet<RoomType> RoomTypes { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}