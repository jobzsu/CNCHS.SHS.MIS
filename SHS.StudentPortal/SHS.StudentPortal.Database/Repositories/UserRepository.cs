using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> CreateUser(User user, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(user, cancellationToken);
    }

    public async Task DeleteUser(User user, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(user, cancellationToken);
    }

    public async Task<User?> GetUserById(Guid userId, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.UserAccount)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken) :
            GetAll()
            .Include(x => x.UserAccount)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken));
    }

    public async Task<User> UpdateUser(User user, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(user, cancellationToken);
    }
}
