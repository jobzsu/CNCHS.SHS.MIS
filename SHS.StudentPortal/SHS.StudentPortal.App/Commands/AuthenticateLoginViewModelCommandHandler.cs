using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class AuthenticateLoginViewModelCommandHandler :
    ICommandHandler<AuthenticateLoginViewModelCommand, UserPrincipalViewModel>
{
    private readonly ILogger<AuthenticateLoginViewModelCommandHandler> _logger;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public AuthenticateLoginViewModelCommandHandler(ILogger<AuthenticateLoginViewModelCommandHandler> logger,
        IBCryptAuthProvider bCryptAuthProvider,
        IUserAccountRepository userAccountRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _bCryptAuthProvider = bCryptAuthProvider;
        _userAccountRepository = userAccountRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel<UserPrincipalViewModel>> Handle(AuthenticateLoginViewModelCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error = null;

        try
        {
            var username = request.username.Trim().ToLower();

            var userAccount = await _userAccountRepository
                .GetUserAccountByUsernameAndType(request.userAccountType,
                    username, cancellationToken);

            if (userAccount is not null)
            {
                // Verify password
                var passwordValid = _bCryptAuthProvider.VerifyPassword(request.password, userAccount.PasswordHash);

                if (passwordValid)
                {
                    if (!userAccount.IsActive)
                    {
                        error = new ErrorModel(nameof(UnauthorizedAccessException),
                            userAccount.InactiveReason ?? "Account is Inactive. Contact Admin.");

                        return ResultModel<UserPrincipalViewModel>.Fail(error);
                    }
                    else
                    {
                        // Update LastLogin
                        var updateUserAccount = await _userAccountRepository.GetUserAccountById(userAccount.Id, cancellationToken, true);
                        updateUserAccount!.LastLogin = DateTime.UtcNow;
                        updateUserAccount!.ModifiedById = Constants.SystemGuid;
                        updateUserAccount!.ModifiedDate = DateTime.UtcNow;

                        await _userAccountRepository.UpdateUserAccount(updateUserAccount, cancellationToken);
                        await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                        var retVal = new UserPrincipalViewModel()
                        {
                            UserId = userAccount.Id,
                            Username = userAccount.Username,
                            Firstname = userAccount.User.FirstName,
                            Middlename = userAccount.User.MiddleName,
                            Lastname = userAccount.User.LastName,
                            RoleType = userAccount.User.RoleType,
                        };

                        return ResultModel<UserPrincipalViewModel>.Success(retVal);
                    }
                }
                else
                {
                    error = new ErrorModel(nameof(UnauthorizedAccessException), "Invalid Username/Password");

                    return ResultModel<UserPrincipalViewModel>.Fail(error);
                }
            }
            else
            {
                error = new ErrorModel(nameof(UnauthorizedAccessException), "Invalid Username/Password");

                return ResultModel<UserPrincipalViewModel>.Fail(error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while authenticating user: {ex.Message}");

            error = new ErrorModel(nameof(UnauthorizedAccessException), "An error occurred while authenticating user");

            return ResultModel<UserPrincipalViewModel>.Fail(error);
        }
    }
}
