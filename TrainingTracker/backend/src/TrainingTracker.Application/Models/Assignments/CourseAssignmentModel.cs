using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Application.Models.Assignments;

public class CourseAssignmentModel
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
    public DateTime? DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public AssignmentStatus Status { get; set; }
    public string? Notes { get; set; }
}
