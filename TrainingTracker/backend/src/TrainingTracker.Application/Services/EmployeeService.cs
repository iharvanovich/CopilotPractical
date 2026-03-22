using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Employees;
using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Employee> _repo;

    public EmployeeService(IRepository<Employee> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<EmployeeModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var employees = await _repo.GetAllAsync(cancellationToken, nameof(Employee.CourseAssignments));
        return employees.Select(e => new EmployeeModel
        {
            Id = e.Id,
            FullName = e.FullName,
            Email = e.Email
        });
    }

    public async Task<EmployeeDetailsModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var employee = await _repo.GetByIdAsync(id, cancellationToken, nameof(Employee.CourseAssignments));
        if (employee == null) return null;

        var details = new EmployeeDetailsModel
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Assignments = employee.CourseAssignments.Select(a => new CourseAssignmentSummaryModel
            {
                Id = a.Id,
                CourseId = a.CourseId,
                CourseTitle = a.Course?.Title ?? string.Empty,
                Status = a.Status.ToString(),
                DueAt = a.DueAt
            }).ToList()
        };

        return details;
    }

    public async Task<EmployeeModel> CreateAsync(EmployeeModel model, CancellationToken cancellationToken)
    {
        var names = model.FullName?.Split(' ', 2) ?? Array.Empty<string>();
        var entity = new Employee
        {
            FirstName = names.FirstOrDefault() ?? string.Empty,
            LastName = names.ElementAtOrDefault(1) ?? string.Empty,
            Email = model.Email
        };

        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        model.Id = entity.Id;
        return model;
    }

    public async Task<EmployeeModel> UpdateAsync(EmployeeModel model, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(model.Id, cancellationToken);
        if (entity == null) throw new KeyNotFoundException("Employee not found");

        var names = model.FullName?.Split(' ', 2) ?? Array.Empty<string>();
        entity.FirstName = names.FirstOrDefault() ?? string.Empty;
        entity.LastName = names.ElementAtOrDefault(1) ?? string.Empty;
        entity.Email = model.Email;

        await _repo.UpdateAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        return model;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity == null) return;

        await _repo.DeleteAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}
