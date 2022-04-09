using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;


internal static class ApplicationDbContextOptionsFactory
{
    private static readonly string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Initial Catalog=HotelManagement;Trusted_Connection=True;";


    public static void Make(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(ConnectionString);
}
