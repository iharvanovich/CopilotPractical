using TrainingTracker.Application.Models.Assignments;

namespace TrainingTracker.Application.Interfaces;

public interface ICourseAssignmentService
{
    Task<IEnumerable<CourseAssignmentModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<CourseAssignmentModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CourseAssignmentModel> CreateAsync(CreateAssignmentModel model, CancellationToken cancellationToken);
    Task UpdateStatusAsync(UpdateAssignmentStatusModel model, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
