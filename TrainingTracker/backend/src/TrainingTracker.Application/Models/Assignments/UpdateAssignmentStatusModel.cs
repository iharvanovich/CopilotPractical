using System.ComponentModel.DataAnnotations;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Application.Models.Assignments;

public class UpdateAssignmentStatusModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public AssignmentStatus Status { get; set; }

    public DateTime? CompletedAt { get; set; }
}
