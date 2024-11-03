using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdatePasswordCommandHandler
    : ICommandHandler<UpdatePasswordCommand>
{
    private readonly ILogger<UpdatePasswordCommandHandler> _logger;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public UpdatePasswordCommandHandler(ILogger<UpdatePasswordCommandHandler> logger,
        IUserRepository userRepository,
        IBCryptAuthProvider bCryptAuthProvider,
        IUserAccountRepository userAccountRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _bCryptAuthProvider = bCryptAuthProvider;
        _userAccountRepository = userAccountRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var userAccount = await _userAccountRepository.GetUserAccountById(request.userId, cancellationToken, true);

            if(userAccount is null)
            {
                error = new(nameof(KeyNotFoundException), "User not found.");

                return ResultModel.Fail(error);
            }

            var newHashedPassword = _bCryptAuthProvider.EncryptPassword(request.newPassword);

            var updatedUserAccountPassword =
                userAccount.UpdatePassword(newHashedPassword, request.userId);

            await _userAccountRepository.UpdateUserAccount(updatedUserAccountPassword, cancellationToken);

            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating password.");

            error = new(nameof(ex), "Error updating password.");

            return ResultModel.Fail(error);
        }
    }
}
