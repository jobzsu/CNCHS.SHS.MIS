using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdateInstructorPasswordCommandHandler
    : ICommandHandler<UpdateInstructorPasswordCommand>
{
    private readonly ILogger<UpdateInstructorPasswordCommandHandler> _logger;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public UpdateInstructorPasswordCommandHandler(ILogger<UpdateInstructorPasswordCommandHandler> logger,
        IInstructorInfoRepository instructorInfoRepository,
        IBCryptAuthProvider bCryptAuthProvider,
        IBaseUnitOfWork baseUnitOfWork,
        IUserAccountRepository userAccountRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
        _bCryptAuthProvider = bCryptAuthProvider;
        _baseUnitOfWork = baseUnitOfWork;
        _userAccountRepository = userAccountRepository;
    }

    public async Task<ResultModel> Handle(UpdateInstructorPasswordCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            using (var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    var instructorInfo = await _instructorInfoRepository.GetInstructorInfoById(request.instructorId, true, cancellationToken);

                    if (instructorInfo is null)
                    {
                        error = new(nameof(KeyNotFoundException), "Instructor not found.");

                        return ResultModel.Fail(error);
                    }

                    string newPasswordHash = _bCryptAuthProvider.EncryptPassword(request.newPassword);

                    instructorInfo.User.UserAccount.UpdatePassword(newPasswordHash, request.updatedBy);

                    await _userAccountRepository.UpdateUserAccount(instructorInfo.User.UserAccount, cancellationToken);

                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);

                    return ResultModel.Success();
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Instructor password");

            error = new(nameof(ex), "Error updating Instructor password");

            return ResultModel.Fail(error);
        }
    }
}
