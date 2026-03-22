using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingTracker.Infrastructure.Persistence;
using TrainingTracker.Infrastructure.Repositories;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Infrastructure.Services;

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

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        // Database initializer
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        return services;
    }

    /// <summary>
    /// Run database initialization (migrations + seeding). Intended to be called at application startup.
    /// </summary>
    public static async Task InitializeDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await initializer.InitializeAsync(cancellationToken);
    }
}
