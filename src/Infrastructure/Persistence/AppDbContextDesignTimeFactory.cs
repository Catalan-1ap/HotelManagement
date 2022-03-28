using Microsoft.EntityFrameworkCore.Design;


namespace Infrastructure.Persistence;


internal class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args) =>
        new ApplicationDbContextFactory().Create();
}
