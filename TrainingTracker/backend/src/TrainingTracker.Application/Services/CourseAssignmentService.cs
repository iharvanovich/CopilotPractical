using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Assignments;
using TrainingTracker.Domain.Entities;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Application.Services;

public class CourseAssignmentService : ICourseAssignmentService
{
    private readonly IRepository<CourseAssignment> _repo;
    private readonly IRepository<Employee> _employeeRepo;
    private readonly IRepository<Course> _courseRepo;

    public CourseAssignmentService(IRepository<CourseAssignment> repo, IRepository<Employee> employeeRepo, IRepository<Course> courseRepo)
    {
        _repo = repo;
        _employeeRepo = employeeRepo;
        _courseRepo = courseRepo;
    }

    public async Task<IEnumerable<CourseAssignmentModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var assignments = await _repo.GetAllAsync(cancellationToken, nameof(CourseAssignment.Employee), nameof(CourseAssignment.Course));

        // Detect overdue status in memory
        foreach (var a in assignments)
        {
            if (a.Status != AssignmentStatus.Completed && a.DueAt.HasValue && a.DueAt.Value < DateTime.UtcNow)
            {
                a.Status = AssignmentStatus.Overdue;
            }
        }

        return assignments.Select(a => new CourseAssignmentModel
        {
            Id = a.Id,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee?.FullName ?? string.Empty,
            CourseId = a.CourseId,
            CourseTitle = a.Course?.Title ?? string.Empty,
            AssignedAt = a.AssignedAt,
            DueAt = a.DueAt,
            CompletedAt = a.CompletedAt,
            Status = a.Status,
            Notes = a.Notes
        });
    }

    public async Task<CourseAssignmentModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var a = await _repo.GetByIdAsync(id, cancellationToken, nameof(CourseAssignment.Employee), nameof(CourseAssignment.Course));
        if (a == null) return null;

        if (a.Status != AssignmentStatus.Completed && a.DueAt.HasValue && a.DueAt.Value < DateTime.UtcNow)
        {
            a.Status = AssignmentStatus.Overdue;
            await _repo.UpdateAsync(a, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);
        }

        return new CourseAssignmentModel
        {
            Id = a.Id,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee?.FullName ?? string.Empty,
            CourseId = a.CourseId,
            CourseTitle = a.Course?.Title ?? string.Empty,
            AssignedAt = a.AssignedAt,
            DueAt = a.DueAt,
            CompletedAt = a.CompletedAt,
            Status = a.Status,
            Notes = a.Notes
        };
    }

    public async Task<CourseAssignmentModel> CreateAsync(CreateAssignmentModel model, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepo.GetByIdAsync(model.EmployeeId, cancellationToken);
        var course = await _courseRepo.GetByIdAsync(model.CourseId, cancellationToken);
        if (employee == null) throw new KeyNotFoundException("Employee not found");
        if (course == null) throw new KeyNotFoundException("Course not found");

        var assignment = new CourseAssignment
        {
            EmployeeId = model.EmployeeId,
            CourseId = model.CourseId,
            AssignedAt = DateTime.UtcNow,
            DueAt = model.DueAt,
            Status = AssignmentStatus.Assigned,
            Notes = model.Notes
        };

        await _repo.AddAsync(assignment, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        var created = await _repo.GetByIdAsync(assignment.Id, cancellationToken, nameof(CourseAssignment.Employee), nameof(CourseAssignment.Course));

        return new CourseAssignmentModel
        {
            Id = created!.Id,
            EmployeeId = created.EmployeeId,
            EmployeeName = created.Employee?.FullName ?? string.Empty,
            CourseId = created.CourseId,
            CourseTitle = created.Course?.Title ?? string.Empty,
            AssignedAt = created.AssignedAt,
            DueAt = created.DueAt,
            CompletedAt = created.CompletedAt,
            Status = created.Status,
            Notes = created.Notes
        };
    }

    public async Task UpdateStatusAsync(UpdateAssignmentStatusModel model, CancellationToken cancellationToken)
    {
        var a = await _repo.GetByIdAsync(model.Id, cancellationToken);
        if (a == null) throw new KeyNotFoundException("Assignment not found");

        a.Status = model.Status;
        if (model.Status == AssignmentStatus.Completed)
        {
            a.CompletedAt = model.CompletedAt ?? DateTime.UtcNow;
        }

        a.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(a, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var a = await _repo.GetByIdAsync(id, cancellationToken);
        if (a == null) return;

        await _repo.DeleteAsync(a, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}
