using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.Models.Assignments;

public class CreateAssignmentModel
{
    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public Guid CourseId { get; set; }

    public DateTime? DueAt { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
