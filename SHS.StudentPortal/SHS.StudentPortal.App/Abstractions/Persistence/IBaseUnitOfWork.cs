namespace SHS.StudentPortal.App.Abstractions.Persistence;

public interface IBaseUnitOfWork
{
    void SaveChanges();

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
