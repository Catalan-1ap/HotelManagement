using System;
using Application.Interfaces;
using Domain.Entities;
using EntityFrameworkCore.Testing.NSubstitute;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Application.UnitTests.Mocks;


public class MockedContext : DbContext, IApplicationDbContext
{
    public static IApplicationDbContext Make(string dbName)
    {
        var options = new DbContextOptionsBuilder<MockedContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return Create.MockedDbContextFor<MockedContext>(options);
    }


    public virtual DbSet<Room> Rooms { get; set; } = null!;
    public virtual DbSet<RoomReport> RoomReports { get; set; } = null!;
    public virtual DbSet<Person> Persons { get; set; } = null!;
    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Cleaner> Cleaners { get; set; } = null!;
    public virtual DbSet<Floor> Floors { get; set; } = null!;
    public virtual DbSet<FloorCleaner> FloorCleaners { get; set; } = null!;
    public virtual DbSet<RoomType> RoomTypes { get; set; } = null!;


    public MockedContext(DbContextOptions<MockedContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Ignore query filter
        modelBuilder.Entity<Client>().HasQueryFilter(c => true);
        
        base.OnModelCreating(modelBuilder);
    }
}
