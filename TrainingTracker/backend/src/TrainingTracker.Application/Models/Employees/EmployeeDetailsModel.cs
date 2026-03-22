using System.Collections.Generic;

namespace TrainingTracker.Application.Models.Employees;

public class EmployeeDetailsModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }

    // A minimal list of assignments for the employee
    public IEnumerable<CourseAssignmentSummaryModel> Assignments { get; set; } = Array.Empty<CourseAssignmentSummaryModel>();
}

public class CourseAssignmentSummaryModel
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime? DueAt { get; set; }
}
