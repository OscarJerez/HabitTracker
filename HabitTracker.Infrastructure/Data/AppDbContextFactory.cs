using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Lösning: ladda korrekt sökväg till appsettings.json
        string basePath = AppContext.BaseDirectory;


        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("'DefaultConnection' is missing in appsettings.json.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
