using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Application.UnitTests.Mocks;


public class MockedContext : DbContext, IApplicationDbContext
{
    public MockedContext(DbContextOptions<MockedContext> options) : base(options) { }


    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomReport> RoomReports { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Cleaner> Cleaners { get; set; } = null!;
    public DbSet<Floor> Floors { get; set; } = null!;
    public DbSet<CleaningSchedule> CleaningSchedule { get; set; } = null!;
    public DbSet<RoomType> RoomTypes { get; set; } = null!;


    public static MockedContext Make(string dbName)
    {
        var options = new DbContextOptionsBuilder<MockedContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new(options);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
