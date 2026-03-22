using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Domain.Entities;

public class CourseAssignment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // Foreign keys
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // Assignment details
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public AssignmentStatus Status { get; set; } = AssignmentStatus.Assigned;

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
