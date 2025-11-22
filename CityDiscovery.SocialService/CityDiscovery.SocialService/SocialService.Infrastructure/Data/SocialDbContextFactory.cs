using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SocialService.Infrastructure.Data;

namespace CityDiscovery.SocialService.SocialService.Infrastructure.Data
{
    // EF Core CLI (dotnet ef) çalışırken DbContext'i oluşturmaya yarar
    public class SocialDbContextFactory : IDesignTimeDbContextFactory<SocialDbContext>
    {
        public SocialDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            var basePath = Directory.GetCurrentDirectory(); // .csproj'in çalıştığı klasör
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var connStr = config.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("Connection string 'DefaultConnection' bulunamadı.");

            var options = new DbContextOptionsBuilder<SocialDbContext>()
                .UseSqlServer(connStr)
                .Options;

            return new SocialDbContext(options);
        }
    }
}
