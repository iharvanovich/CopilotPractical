using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainingTracker.Domain.Entities;
using TrainingTracker.Domain.Enums;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Services;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly AppDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(AppDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.MigrateAsync(cancellationToken);

            if (await _context.Employees.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Database already seeded; skipping.");
                return;
            }

            _logger.LogInformation("Seeding TrainingTracker database...");

            // Seed employees
            var now = DateTime.UtcNow;
            var employees = new List<Employee>
            {
                new Employee { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@contoso.com", CreatedAt = now.AddDays(-90) },
                new Employee { FirstName = "Bob", LastName = "Martinez", Email = "bob.martinez@contoso.com", CreatedAt = now.AddDays(-80) },
                new Employee { FirstName = "Carla", LastName = "Nguyen", Email = "carla.nguyen@contoso.com", CreatedAt = now.AddDays(-70) },
                new Employee { FirstName = "Daniel", LastName = "Smith", Email = "daniel.smith@contoso.com", CreatedAt = now.AddDays(-60) },
                new Employee { FirstName = "Evelyn", LastName = "Garcia", Email = "evelyn.garcia@contoso.com", CreatedAt = now.AddDays(-50) },
                new Employee { FirstName = "Frank", LastName = "O'Connor", Email = "frank.oconnor@contoso.com", CreatedAt = now.AddDays(-40) }
            };

            await _context.Employees.AddRangeAsync(employees, cancellationToken);

            // Categories
            var categories = new List<CourseCategory>
            {
                new CourseCategory { Name = "Compliance", Description = "Mandatory regulatory and company policy training.", CreatedAt = now.AddDays(-200) },
                new CourseCategory { Name = "Leadership", Description = "Courses focused on management, coaching and career growth.", CreatedAt = now.AddDays(-180) },
                new CourseCategory { Name = "Technical", Description = "Role-specific technical skills and tools.", CreatedAt = now.AddDays(-150) },
                new CourseCategory { Name = "Onboarding", Description = "Introductory courses for new hires and role orientation.", CreatedAt = now.AddDays(-120) }
            };

            await _context.CourseCategories.AddRangeAsync(categories, cancellationToken);

            // Courses
            var courses = new List<Course>
            {
                new Course { Title = "Data Protection & Privacy", Description = "Covers GDPR and company data handling policies.", CourseCategory = categories[0], EstimatedHours = 2, CreatedAt = now.AddDays(-100) },
                new Course { Title = "Workplace Safety", Description = "General workplace safety and emergency procedures.", CourseCategory = categories[0], EstimatedHours = 1, CreatedAt = now.AddDays(-95) },
                new Course { Title = "Effective 1:1s for Managers", Description = "How to run productive one-on-one meetings.", CourseCategory = categories[1], EstimatedHours = 3, CreatedAt = now.AddDays(-90) },
                new Course { Title = "Inclusive Leadership", Description = "Building inclusive teams and reducing bias.", CourseCategory = categories[1], EstimatedHours = 2, CreatedAt = now.AddDays(-85) },
                new Course { Title = "C# Advanced Patterns", Description = "Design patterns and best practices for C# developers.", CourseCategory = categories[2], EstimatedHours = 5, CreatedAt = now.AddDays(-60) },
                new Course { Title = "Azure Fundamentals", Description = "Core concepts of cloud computing and Microsoft Azure services.", CourseCategory = categories[2], EstimatedHours = 4, CreatedAt = now.AddDays(-55) },
                new Course { Title = "New Hire Orientation", Description = "Company overview, culture, and systems for new employees.", CourseCategory = categories[3], EstimatedHours = 1, CreatedAt = now.AddDays(-30) },
                new Course { Title = "Git & Source Control", Description = "Practical git workflows used in the company.", CourseCategory = categories[2], EstimatedHours = 2, CreatedAt = now.AddDays(-20) }
            };

            await _context.Courses.AddRangeAsync(courses, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Assignments
            var assignments = new List<CourseAssignment>
            {
                new CourseAssignment { Employee = employees[0], Course = courses[4], AssignedAt = now.AddDays(-1), DueAt = now.AddDays(14), Status = AssignmentStatus.Assigned, Notes = "Recommended for current project.", CreatedAt = now.AddDays(-1) },
                new CourseAssignment { Employee = employees[1], Course = courses[5], AssignedAt = now.AddDays(-7), DueAt = now.AddDays(7), Status = AssignmentStatus.InProgress, Notes = "Started; needs to finish before cloud migration.", CreatedAt = now.AddDays(-7) },
                new CourseAssignment { Employee = employees[2], Course = courses[0], AssignedAt = now.AddDays(-30), DueAt = now.AddDays(-15), CompletedAt = now.AddDays(-20), Status = AssignmentStatus.Completed, Notes = "Completed with high score.", CreatedAt = now.AddDays(-30), UpdatedAt = now.AddDays(-20) },
                new CourseAssignment { Employee = employees[3], Course = courses[1], AssignedAt = now.AddDays(-40), DueAt = now.AddDays(-10), Status = AssignmentStatus.Overdue, Notes = "Follow up with manager.", CreatedAt = now.AddDays(-40) },
                new CourseAssignment { Employee = employees[4], Course = courses[6], AssignedAt = now, DueAt = now.AddDays(30), Status = AssignmentStatus.Assigned, CreatedAt = now },
                new CourseAssignment { Employee = employees[5], Course = courses[7], AssignedAt = now.AddDays(-3), DueAt = now.AddDays(4), Status = AssignmentStatus.InProgress, CreatedAt = now.AddDays(-3) },
                new CourseAssignment { Employee = employees[0], Course = courses[2], AssignedAt = now.AddDays(-60), DueAt = now.AddDays(-45), CompletedAt = now.AddDays(-50), Status = AssignmentStatus.Completed, CreatedAt = now.AddDays(-60), UpdatedAt = now.AddDays(-50) },
                new CourseAssignment { Employee = employees[1], Course = courses[3], AssignedAt = now.AddDays(-20), DueAt = now.AddDays(-5), Status = AssignmentStatus.Overdue, Notes = "Manager escalated.", CreatedAt = now.AddDays(-20) },
                new CourseAssignment { Employee = employees[2], Course = courses[5], AssignedAt = now.AddDays(-2), DueAt = now.AddDays(10), Status = AssignmentStatus.Assigned, CreatedAt = now.AddDays(-2) },
                new CourseAssignment { Employee = employees[3], Course = courses[4], AssignedAt = now.AddDays(-5), DueAt = now.AddDays(2), Status = AssignmentStatus.InProgress, CreatedAt = now.AddDays(-5) }
            };

            // Ensure assignment CreatedAt/UpdatedAt align with AssignedAt/CompletedAt where appropriate
            foreach (var a in assignments)
            {
                a.CreatedAt = a.AssignedAt;
                if (a.CompletedAt.HasValue)
                    a.UpdatedAt = a.CompletedAt;
            }

            await _context.CourseAssignments.AddRangeAsync(assignments, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Seeding complete.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}
