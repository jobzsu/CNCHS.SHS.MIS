namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IBaseRepository<T> where T : class, new()
{
    IQueryable<T> GetAll();

    Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    T Insert(T entity);

    Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default);

    T Update(T entity);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    void Delete(T entity);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
