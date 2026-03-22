using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Courses;
using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Application.Services;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _repo;

    public CourseService(IRepository<Course> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CourseModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var courses = await _repo.GetAllAsync(cancellationToken, nameof(Course.CourseCategory));
        return courses.Select(c => new CourseModel
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CourseCategoryId = c.CourseCategoryId,
            CourseCategoryName = c.CourseCategory?.Name,
            EstimatedHours = c.EstimatedHours
        });
    }

    public async Task<CourseModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(id, cancellationToken, nameof(Course.CourseCategory), nameof(Course.CourseAssignments));
        if (c == null) return null;

        return new CourseModel
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CourseCategoryId = c.CourseCategoryId,
            CourseCategoryName = c.CourseCategory?.Name,
            EstimatedHours = c.EstimatedHours
        };
    }

    public async Task<CourseModel> CreateAsync(CreateCourseModel model, CancellationToken cancellationToken)
    {
        var entity = new Course
        {
            Title = model.Title,
            Description = model.Description,
            CourseCategoryId = model.CourseCategoryId,
            EstimatedHours = model.EstimatedHours
        };

        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        return new CourseModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CourseCategoryId = entity.CourseCategoryId,
            EstimatedHours = entity.EstimatedHours
        };
    }

    public async Task<CourseModel> UpdateAsync(UpdateCourseModel model, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(model.Id, cancellationToken);
        if (entity == null) throw new KeyNotFoundException("Course not found");

        entity.Title = model.Title;
        entity.Description = model.Description;
        entity.CourseCategoryId = model.CourseCategoryId;
        entity.EstimatedHours = model.EstimatedHours;

        await _repo.UpdateAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        return new CourseModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CourseCategoryId = entity.CourseCategoryId,
            EstimatedHours = entity.EstimatedHours
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity == null) return;

        await _repo.DeleteAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}
