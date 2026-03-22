using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Application.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken, params string[] includes);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, params string[] includes);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
