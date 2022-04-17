using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;


internal sealed class ReadOnlyApplicationDbContext : IReadOnlyApplicationDbContext
{
    private readonly IApplicationDbContext _dbContext;


    public ReadOnlyApplicationDbContext(IApplicationDbContext dbContext) => _dbContext = dbContext;


    public IQueryable<Room> Rooms => _dbContext.Rooms.AsNoTrackingWithIdentityResolution();
    public IQueryable<RoomReport> RoomReports => _dbContext.RoomReports.AsNoTrackingWithIdentityResolution();
    public IQueryable<Person> Persons => _dbContext.Persons.AsNoTrackingWithIdentityResolution();
    public IQueryable<Client> Clients => _dbContext.Clients.AsNoTrackingWithIdentityResolution();
    public IQueryable<Cleaner> Cleaners => _dbContext.Cleaners.AsNoTrackingWithIdentityResolution();
    public IQueryable<Floor> Floors => _dbContext.Floors.AsNoTrackingWithIdentityResolution();
    public IQueryable<CleaningSchedule> CleaningSchedule => _dbContext.CleaningSchedule.AsNoTrackingWithIdentityResolution();
    public IQueryable<RoomType> RoomTypes => _dbContext.RoomTypes.AsNoTrackingWithIdentityResolution();


    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

    public void Dispose() => _dbContext.Dispose();
}
