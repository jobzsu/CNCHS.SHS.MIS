using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdatePersonalInfoCommandHandler
    : ICommandHandler<UpdatePersonalInfoCommand>
{
    private readonly ILogger<UpdatePersonalInfoCommandHandler> _logger;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public UpdatePersonalInfoCommandHandler(ILogger<UpdatePersonalInfoCommandHandler> logger,
        IUserAccountRepository userAccountRepository,
        IUserRepository userRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _userAccountRepository = userAccountRepository;
        _userRepository = userRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var userAccount = await _userAccountRepository.GetUserAccountById(request.userId, cancellationToken, true);

            if(userAccount is null)
            {
                error = new(nameof(KeyNotFoundException), "User Account not found.");

                return ResultModel.Fail(error);
            }

            var middleName = string.IsNullOrWhiteSpace(request.model.MiddleName) ?
                null : request.model.MiddleName;

            var updatedUser = userAccount.User.Update(request.model.FirstName,
                middleName, 
                request.model.LastName, 
                request.userId);

            await _userRepository.UpdateUser(updatedUser, cancellationToken);
            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating personal info.");

            error = new(nameof(ex), "Error updating personal info.");

            return ResultModel.Fail(error);
        }
    }
}
