using Microsoft.EntityFrameworkCore;
using TrainingTracker.Domain.Entities;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<CourseCategory> CourseCategories { get; set; } = null!;
    public DbSet<CourseAssignment> CourseAssignments { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Default to SQLite file in the application folder. In real apps, use configuration.
            optionsBuilder.UseSqlite("Data Source=trainingtracker.db");
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(320);

            entity.HasMany(e => e.CourseAssignments)
                  .WithOne(ca => ca.Employee)
                  .HasForeignKey(ca => ca.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // CourseCategory
        modelBuilder.Entity<CourseCategory>(entity =>
        {
            entity.ToTable("CourseCategories");
            entity.HasKey(cc => cc.Id);
            entity.Property(cc => cc.Name).IsRequired().HasMaxLength(200);
            entity.Property(cc => cc.Description).HasMaxLength(1000);

            entity.HasMany(cc => cc.Courses)
                  .WithOne(c => c.CourseCategory)
                  .HasForeignKey(c => c.CourseCategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Course
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(300);
            entity.Property(c => c.Description).HasMaxLength(2000);
            entity.Property(c => c.EstimatedHours);

            entity.HasMany(c => c.CourseAssignments)
                  .WithOne(ca => ca.Course)
                  .HasForeignKey(ca => ca.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // CourseAssignment
        modelBuilder.Entity<CourseAssignment>(entity =>
        {
            entity.ToTable("CourseAssignments");
            entity.HasKey(ca => ca.Id);

            entity.Property(ca => ca.AssignedAt).IsRequired();
            entity.Property(ca => ca.DueAt);
            entity.Property(ca => ca.CompletedAt);
            entity.Property(ca => ca.Notes).HasMaxLength(1000);

            entity.Property(ca => ca.Status)
                  .HasConversion<int>()
                  .IsRequired()
                  .HasDefaultValue(TrainingTracker.Domain.Enums.AssignmentStatus.Assigned);

            entity.HasIndex(ca => new { ca.EmployeeId, ca.CourseId });
        });
    }
}
