using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Dashboard;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IRepository<Domain.Entities.Employee> _employeeRepo;
    private readonly IRepository<Domain.Entities.Course> _courseRepo;
    private readonly IRepository<Domain.Entities.CourseAssignment> _assignmentRepo;

    public DashboardService(IRepository<Domain.Entities.Employee> employeeRepo,
        IRepository<Domain.Entities.Course> courseRepo,
        IRepository<Domain.Entities.CourseAssignment> assignmentRepo)
    {
        _employeeRepo = employeeRepo;
        _courseRepo = courseRepo;
        _assignmentRepo = assignmentRepo;
    }

    public async Task<DashboardSummaryModel> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var employees = await _employeeRepo.GetAllAsync(cancellationToken);
        var courses = await _courseRepo.GetAllAsync(cancellationToken);
        var assignments = await _assignmentRepo.GetAllAsync(cancellationToken);

        // Detect overdue in-memory
        foreach (var a in assignments)
        {
            if (a.Status != AssignmentStatus.Completed && a.DueAt.HasValue && a.DueAt.Value < DateTime.UtcNow)
                a.Status = AssignmentStatus.Overdue;
        }

        return new DashboardSummaryModel
        {
            TotalEmployees = employees.Count,
            TotalCourses = courses.Count,
            TotalAssignments = assignments.Count,
            AssignedCount = assignments.Count(a => a.Status == AssignmentStatus.Assigned),
            InProgressCount = assignments.Count(a => a.Status == AssignmentStatus.InProgress),
            CompletedCount = assignments.Count(a => a.Status == AssignmentStatus.Completed),
            OverdueCount = assignments.Count(a => a.Status == AssignmentStatus.Overdue)
        };
    }
}
