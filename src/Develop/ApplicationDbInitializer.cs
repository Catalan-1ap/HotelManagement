using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Develop;


public static class ApplicationDbInitializer
{
    public static void ReCreate()
    {
        using var dbContext = ApplicationDbContextFactory.CreateDbContext();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }


    public static bool IsNotExists()
    {
        using var dbContext = ApplicationDbContextFactory.CreateDbContext();

        return !dbContext.Database.CanConnect();
    }
}
