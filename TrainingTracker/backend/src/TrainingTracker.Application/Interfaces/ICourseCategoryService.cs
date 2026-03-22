using TrainingTracker.Application.Models.Courses;

namespace TrainingTracker.Application.Interfaces;

public interface ICourseCategoryService
{
    Task<IEnumerable<CourseCategoryModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<CourseCategoryModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CourseCategoryModel> CreateAsync(CourseCategoryModel model, CancellationToken cancellationToken);
    Task<CourseCategoryModel> UpdateAsync(CourseCategoryModel model, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
