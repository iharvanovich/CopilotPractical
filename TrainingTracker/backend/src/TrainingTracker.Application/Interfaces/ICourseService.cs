using TrainingTracker.Application.Models.Courses;

namespace TrainingTracker.Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<CourseModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CourseModel> CreateAsync(CreateCourseModel model, CancellationToken cancellationToken);
    Task<CourseModel> UpdateAsync(UpdateCourseModel model, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
