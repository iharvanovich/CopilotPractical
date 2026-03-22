using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Courses;
using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Application.Services;

public class CourseCategoryService : ICourseCategoryService
{
    private readonly IRepository<CourseCategory> _repo;

    public CourseCategoryService(IRepository<CourseCategory> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CourseCategoryModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _repo.GetAllAsync(cancellationToken, nameof(CourseCategory.Courses));
        return categories.Select(c => new CourseCategoryModel
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
    }

    public async Task<CourseCategoryModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(id, cancellationToken, nameof(CourseCategory.Courses));
        if (c == null) return null;

        return new CourseCategoryModel
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        };
    }

    public async Task<CourseCategoryModel> CreateAsync(CourseCategoryModel model, CancellationToken cancellationToken)
    {
        var entity = new CourseCategory
        {
            Name = model.Name,
            Description = model.Description
        };

        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        model.Id = entity.Id;
        return model;
    }

    public async Task<CourseCategoryModel> UpdateAsync(CourseCategoryModel model, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(model.Id, cancellationToken);
        if (entity == null) throw new KeyNotFoundException("Category not found");

        entity.Name = model.Name;
        entity.Description = model.Description;

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
