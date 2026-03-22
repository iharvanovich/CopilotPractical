using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Domain.Entities;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _set;

    public EfRepository(AppDbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken, params string[] includes)
    {
        IQueryable<T> q = _set.AsNoTracking();
        foreach (var inc in includes)
            q = q.Include(inc);
        return await q.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, params string[] includes)
    {
        IQueryable<T> q = _set;
        foreach (var inc in includes)
            q = q.Include(inc);
        return await q.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _set.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
