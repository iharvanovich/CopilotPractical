using TrainingTracker.Infrastructure.Extensions;
using TrainingTracker.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Register persistence (DbContext) using extension from Infrastructure
builder.Services.AddPersistence(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Ensure database is migrated and seeded
await app.MigrateAndSeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
