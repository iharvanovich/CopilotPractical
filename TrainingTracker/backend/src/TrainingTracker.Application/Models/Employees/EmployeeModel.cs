using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.Models.Employees;

public class EmployeeModel
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }
}
