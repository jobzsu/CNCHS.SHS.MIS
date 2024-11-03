using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;

namespace SHS.StudentPortal.Database.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    private readonly AppDbContext _appDbContext;

    public BaseRepository(AppDbContext appDbContext)
    {
        ArgumentNullException.ThrowIfNull(nameof(appDbContext));

        _appDbContext = appDbContext;
    }

    public void Delete(T entity)
    {
        _appDbContext.Set<T>().Remove(entity);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => Delete(entity), cancellationToken);
    }

    public IQueryable<T> GetAll()
    {
        return _appDbContext.Set<T>();
    }

    public async Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => GetAll(), cancellationToken);
    }

    public T Insert(T entity)
    {
        return InsertAsync(entity).Result;
    }

    public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);

        return entity;
    }

    public T Update(T entity)
    {
        _appDbContext.Set<T>().Update(entity);

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => Update(entity), cancellationToken);
    }
}
