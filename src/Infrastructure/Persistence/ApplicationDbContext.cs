using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;


internal class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomReport> RoomReports { get; set; } = null!;
    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Cleaner> Cleaners { get; set; } = null!;
    public DbSet<Floor> Floors { get; set; } = null!;
    public DbSet<FloorCleaner> FloorCleaners { get; set; } = null!;
    public DbSet<RoomType> RoomTypes { get; set; } = null!;


    public ApplicationDbContext(DbContextOptions options)
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}