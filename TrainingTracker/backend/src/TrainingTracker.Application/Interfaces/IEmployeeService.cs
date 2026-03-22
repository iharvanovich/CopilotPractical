using TrainingTracker.Application.Models.Employees;

namespace TrainingTracker.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<EmployeeDetailsModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<EmployeeModel> CreateAsync(EmployeeModel model, CancellationToken cancellationToken);
    Task<EmployeeModel> UpdateAsync(EmployeeModel model, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
