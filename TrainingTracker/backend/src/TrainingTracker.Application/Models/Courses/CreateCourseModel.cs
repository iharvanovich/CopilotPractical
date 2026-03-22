using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.Models.Courses;

public class CreateCourseModel
{
    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = null!;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public Guid? CourseCategoryId { get; set; }

    public int? EstimatedHours { get; set; }
}
