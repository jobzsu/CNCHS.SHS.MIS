using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IUserAccountRepository
{
    Task<UserAccount?> GetUserAccountByUsername(string username, 
        CancellationToken cancellationToken = default,
        bool shouldTrack = false);

    Task<UserAccount?> GetUserAccountByUsernameAndType(string userAccountType,
        string username,
        CancellationToken cancellationToken = default,
        bool shouldTrack = false);

    Task<UserAccount?> GetUserAccountById(Guid id,
        CancellationToken cancellationToken = default,
        bool shouldTrack = false);

    Task<UserAccount> CreateUserAccount(UserAccount userAccount,
        CancellationToken cancellationToken = default);

    Task<UserAccount> UpdateUserAccount(UserAccount userAccount,
        CancellationToken cancellationToken = default);

    Task DeleteUserAccount(UserAccount userAccount, CancellationToken cancellationToken = default);
}
