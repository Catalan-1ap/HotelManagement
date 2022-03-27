using Microsoft.EntityFrameworkCore.Design;


namespace HotelManagement.Infrastructure.Persistence
{
    internal class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args) => new();
    }
}
