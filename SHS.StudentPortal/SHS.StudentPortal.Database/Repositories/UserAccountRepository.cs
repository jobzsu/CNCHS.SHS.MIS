using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class UserAccountRepository : BaseRepository<UserAccount>, IUserAccountRepository
{
    private readonly AppDbContext _appDbContext;

    public UserAccountRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<UserAccount> CreateUserAccount(UserAccount userAccount, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(userAccount, cancellationToken);
    }

    public async Task DeleteUserAccount(UserAccount userAccount, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(userAccount, cancellationToken);
    }

    public async Task<UserAccount?> GetUserAccountById(Guid id, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            :
            await GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<UserAccount?> GetUserAccountByUsername(string username, 
        CancellationToken cancellationToken = default,
        bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower(), cancellationToken)
            :
            await GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower(), cancellationToken);
    }

    public async Task<UserAccount?> GetUserAccountByUsernameAndType(string userAccountType, string username, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower() && x.User.RoleType.ToLower() == userAccountType, cancellationToken)
            :
            await GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower() && x.User.RoleType.ToLower() == userAccountType, cancellationToken);
    }

    public async Task<UserAccount> UpdateUserAccount(UserAccount userAccount, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(userAccount, cancellationToken);
    }
}
