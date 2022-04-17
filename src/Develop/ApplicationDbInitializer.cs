using Infrastructure.Persistence;


namespace Develop;


public static class ApplicationDbInitializer
{
    public static async Task ReCreate()
    {
        await using var dbContext = ApplicationDbContextFactory.CreateDbContext();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }


    public static async Task Create()
    {
        await using var dbContext = ApplicationDbContextFactory.CreateDbContext();

        await dbContext.Database.EnsureCreatedAsync();
    }
}
