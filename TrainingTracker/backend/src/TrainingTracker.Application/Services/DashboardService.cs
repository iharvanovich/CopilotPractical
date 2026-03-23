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

        // Detect overdue in-memory, treat null as Assigned
        foreach (var a in assignments)
        {
            var status = a.Status ?? AssignmentStatus.Assigned;
            if (status != AssignmentStatus.Completed && a.DueAt.HasValue && a.DueAt.Value < DateTime.UtcNow)
                a.Status = AssignmentStatus.Overdue;
        }

        int assignedCount = assignments.Count(a => (a.Status ?? AssignmentStatus.Assigned) == AssignmentStatus.Assigned);
        int inProgressCount = assignments.Count(a => (a.Status ?? AssignmentStatus.InProgress) == AssignmentStatus.InProgress);
        int completedCount = assignments.Count(a => (a.Status ?? AssignmentStatus.Completed) == AssignmentStatus.Completed);
        int overdueCount = assignments.Count(a => (a.Status ?? AssignmentStatus.Overdue) == AssignmentStatus.Overdue);

        return new DashboardSummaryModel
        {
            TotalEmployees = employees.Count,
            TotalCourses = courses.Count,
            TotalAssignments = assignments.Count,
            AssignedCount = assignedCount,
            InProgressCount = inProgressCount,
            CompletedCount = completedCount,
            OverdueCount = overdueCount
        };
    }
}
