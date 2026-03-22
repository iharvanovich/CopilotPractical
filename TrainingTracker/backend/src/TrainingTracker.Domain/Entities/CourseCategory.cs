using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Domain.Entities;

public class CourseCategory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; set; }

    // Navigation: courses in this category
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
