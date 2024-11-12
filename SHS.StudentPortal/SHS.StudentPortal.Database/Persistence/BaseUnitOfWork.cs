using Microsoft.EntityFrameworkCore.Storage;
using SHS.StudentPortal.App.Abstractions.Persistence;

namespace SHS.StudentPortal.Database.Persistence;

public class BaseUnitOfWork : IBaseUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public BaseUnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _appDbContext.Database.BeginTransaction();
    }

    public void SaveChanges()
    {
        _appDbContext.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
