using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Domain.Entities;

public class Employee
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(320)]
    public string? Email { get; set; }

    // Navigation: assignments assigned to this employee
    public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
