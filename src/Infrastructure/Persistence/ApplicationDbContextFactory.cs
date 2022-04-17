using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Infrastructure.Persistence;


internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args) => CreateDbContext();


    public static ApplicationDbContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder();

        ApplicationDbContextOptionsFactory.Make(builder);

        return new(builder.Options);
    }
}
