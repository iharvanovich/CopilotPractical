namespace TrainingTracker.Application.Models.Courses;

public class CourseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid? CourseCategoryId { get; set; }
    public string? CourseCategoryName { get; set; }
    public int? EstimatedHours { get; set; }
}
