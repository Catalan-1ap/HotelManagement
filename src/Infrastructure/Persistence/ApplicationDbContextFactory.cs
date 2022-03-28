using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;


internal sealed class ApplicationDbContextFactory
{
    private readonly string _connectionString =
        @"Server=(localdb)\mssqllocaldb;Initial Catalog=HotelManagement;Trusted_Connection=True;";
    
    
    public ApplicationDbContext Create()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.UseSqlServer(_connectionString);

        return new(optionsBuilder.Options);
    }
}
