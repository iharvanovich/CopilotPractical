using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrainingTracker.Domain.Entities;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task MigrateAndSeedAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(DbSeeder).FullName ?? "DbSeeder");

        try
        {
            var context = services.GetRequiredService<AppDbContext>();

            // Apply migrations (or ensure created for simple scenarios)
            await context.Database.MigrateAsync(cancellationToken);

            // Seed data if not present
            if (!await context.Employees.AnyAsync(cancellationToken))
            {
                logger.LogInformation("Seeding TrainingTracker database...");
                await SeedAsync(context, cancellationToken);
                logger.LogInformation("Seeding complete.");
            }
            else
            {
                logger.LogInformation("Database already contains seed data; skipping seeding.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            throw;
        }
    }

    private static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken)
    {
        // Employees
        var employees = new List<Employee>
        {
            new Employee { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@contoso.com" },
            new Employee { FirstName = "Bob", LastName = "Martinez", Email = "bob.martinez@contoso.com" },
            new Employee { FirstName = "Carla", LastName = "Nguyen", Email = "carla.nguyen@contoso.com" },
            new Employee { FirstName = "Daniel", LastName = "Smith", Email = "daniel.smith@contoso.com" },
            new Employee { FirstName = "Evelyn", LastName = "Garcia", Email = "evelyn.garcia@contoso.com" },
            new Employee { FirstName = "Frank", LastName = "O'Connor", Email = "frank.oconnor@contoso.com" }
        };

        await context.Employees.AddRangeAsync(employees, cancellationToken);

        // Categories
        var categories = new List<CourseCategory>
        {
            new CourseCategory { Name = "Compliance", Description = "Mandatory regulatory and company policy training." },
            new CourseCategory { Name = "Leadership", Description = "Courses focused on management, coaching and career growth." },
            new CourseCategory { Name = "Technical", Description = "Role-specific technical skills and tools." },
            new CourseCategory { Name = "Onboarding", Description = "Introductory courses for new hires and role orientation." }
        };

        await context.CourseCategories.AddRangeAsync(categories, cancellationToken);

        // Courses
        var courses = new List<Course>
        {
            new Course { Title = "Data Protection & Privacy", Description = "Covers GDPR and company data handling policies.", CourseCategory = categories[0], EstimatedHours = 2 },
            new Course { Title = "Workplace Safety", Description = "General workplace safety and emergency procedures.", CourseCategory = categories[0], EstimatedHours = 1 },
            new Course { Title = "Effective 1:1s for Managers", Description = "How to run productive one-on-one meetings.", CourseCategory = categories[1], EstimatedHours = 3 },
            new Course { Title = "Inclusive Leadership", Description = "Building inclusive teams and reducing bias.", CourseCategory = categories[1], EstimatedHours = 2 },
            new Course { Title = "C# Advanced Patterns", Description = "Design patterns and best practices for C# developers.", CourseCategory = categories[2], EstimatedHours = 5 },
            new Course { Title = "Azure Fundamentals", Description = "Core concepts of cloud computing and Microsoft Azure services.", CourseCategory = categories[2], EstimatedHours = 4 },
            new Course { Title = "New Hire Orientation", Description = "Company overview, culture, and systems for new employees.", CourseCategory = categories[3], EstimatedHours = 1 },
            new Course { Title = "Git & Source Control", Description = "Practical git workflows used in the company.", CourseCategory = categories[2], EstimatedHours = 2 }
        };

        await context.Courses.AddRangeAsync(courses, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        // Create assignments with a mix of statuses
        var now = DateTime.UtcNow;

        var assignments = new List<CourseAssignment>
        {
            // Assigned - future due date
            new CourseAssignment
            {
                Employee = employees[0],
                Course = courses[4],
                AssignedAt = now.AddDays(-1),
                DueAt = now.AddDays(14),
                Status = AssignmentStatus.Assigned,
                Notes = "Recommended for current project." 
            },

            // InProgress - due in future
            new CourseAssignment
            {
                Employee = employees[1],
                Course = courses[5],
                AssignedAt = now.AddDays(-7),
                DueAt = now.AddDays(7),
                Status = AssignmentStatus.InProgress,
                Notes = "Started; needs to finish before cloud migration." 
            },

            // Completed - completed recently
            new CourseAssignment
            {
                Employee = employees[2],
                Course = courses[0],
                AssignedAt = now.AddDays(-30),
                DueAt = now.AddDays(-15),
                CompletedAt = now.AddDays(-20),
                Status = AssignmentStatus.Completed,
                Notes = "Completed with high score." 
            },

            // Overdue - due date passed and not completed
            new CourseAssignment
            {
                Employee = employees[3],
                Course = courses[1],
                AssignedAt = now.AddDays(-40),
                DueAt = now.AddDays(-10),
                Status = AssignmentStatus.Overdue,
                Notes = "Follow up with manager." 
            },

            // Assigned
            new CourseAssignment
            {
                Employee = employees[4],
                Course = courses[6],
                AssignedAt = now,
                DueAt = now.AddDays(30),
                Status = AssignmentStatus.Assigned
            },

            // InProgress
            new CourseAssignment
            {
                Employee = employees[5],
                Course = courses[7],
                AssignedAt = now.AddDays(-3),
                DueAt = now.AddDays(4),
                Status = AssignmentStatus.InProgress
            },

            // Completed
            new CourseAssignment
            {
                Employee = employees[0],
                Course = courses[2],
                AssignedAt = now.AddDays(-60),
                DueAt = now.AddDays(-45),
                CompletedAt = now.AddDays(-50),
                Status = AssignmentStatus.Completed
            },

            // Overdue
            new CourseAssignment
            {
                Employee = employees[1],
                Course = courses[3],
                AssignedAt = now.AddDays(-20),
                DueAt = now.AddDays(-5),
                Status = AssignmentStatus.Overdue,
                Notes = "Manager escalated." 
            },

            // Assigned
            new CourseAssignment
            {
                Employee = employees[2],
                Course = courses[5],
                AssignedAt = now.AddDays(-2),
                DueAt = now.AddDays(10),
                Status = AssignmentStatus.Assigned
            },

            // InProgress
            new CourseAssignment
            {
                Employee = employees[3],
                Course = courses[4],
                AssignedAt = now.AddDays(-5),
                DueAt = now.AddDays(2),
                Status = AssignmentStatus.InProgress
            }
        };

        await context.CourseAssignments.AddRangeAsync(assignments, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
