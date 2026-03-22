using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities;

public class Course : BaseEntity
{
    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = null!;

    [MaxLength(2000)]
    public string? Description { get; set; }

    // Optional: reference to a category
    public Guid? CourseCategoryId { get; set; }
    public CourseCategory? CourseCategory { get; set; }

    // Estimated duration in hours
    public int? EstimatedHours { get; set; }

    // Navigation: assignments for this course
    public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
}
