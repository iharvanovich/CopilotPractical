namespace TrainingTracker.Application.Models.Courses;

public class CourseCategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
