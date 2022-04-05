using System.Linq;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;


internal sealed class ReadOnlyApplicationDbContext : IReadOnlyApplicationDbContext
{
    private readonly IApplicationDbContext _dbContext;

    public IQueryable<Room> Rooms => _dbContext.Rooms.AsNoTrackingWithIdentityResolution();
    public IQueryable<RoomReport> RoomReports => _dbContext.RoomReports.AsNoTrackingWithIdentityResolution();
    public IQueryable<Person> Persons => _dbContext.Persons.AsNoTrackingWithIdentityResolution();
    public IQueryable<Client> Clients => _dbContext.Clients.AsNoTrackingWithIdentityResolution();
    public IQueryable<Cleaner> Cleaners => _dbContext.Cleaners.AsNoTrackingWithIdentityResolution();
    public IQueryable<Floor> Floors => _dbContext.Floors.AsNoTrackingWithIdentityResolution();
    public IQueryable<FloorCleaner> FloorCleaners => _dbContext.FloorCleaners.AsNoTrackingWithIdentityResolution();
    public IQueryable<RoomType> RoomTypes => _dbContext.RoomTypes.AsNoTrackingWithIdentityResolution();


    public ReadOnlyApplicationDbContext(IApplicationDbContext dbContext) => _dbContext = dbContext;
}
