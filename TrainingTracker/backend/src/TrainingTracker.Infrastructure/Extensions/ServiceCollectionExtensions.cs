using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers persistence services and configures the SQLite DbContext using configuration.
    /// Reads connection string key "DefaultConnection" and falls back to a local file.
    /// Keeps EF Core configuration inside the Infrastructure project.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=trainingtracker.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }
}
