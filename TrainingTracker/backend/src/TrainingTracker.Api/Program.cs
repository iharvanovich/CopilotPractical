using TrainingTracker.Application.Extensions;
using TrainingTracker.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register persistence (DbContext) using extension from Infrastructure
builder.Services.AddPersistence(builder.Configuration);

// Register application services
builder.Services.AddApplicationServices();

// Configure CORS - simple default policy for development/course project
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Ensure database is migrated and seeded
await app.Services.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("DefaultCors");

app.MapControllers();

app.Run();
