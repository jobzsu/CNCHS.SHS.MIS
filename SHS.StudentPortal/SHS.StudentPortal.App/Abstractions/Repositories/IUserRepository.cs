using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user, CancellationToken cancellationToken = default);

    Task DeleteUser(User user, CancellationToken cancellationToken = default);

    Task<User> UpdateUser(User user, CancellationToken cancellationToken = default);

    Task<User?> GetUserById(Guid userId, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<User?> GetUserByUserAccountId(Guid userAccountId, CancellationToken cancellationToken = default, bool shouldTrack = false);

    Task<User?> GetUserByFirstAndLastName(string firstName, string lastName, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<User>?> GetAllList(CancellationToken cancellationToken = default, bool shouldTrack = false);
}
