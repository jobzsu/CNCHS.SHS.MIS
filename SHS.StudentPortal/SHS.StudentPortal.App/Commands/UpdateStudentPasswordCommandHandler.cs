using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdateStudentPasswordCommandHandler :
    ICommandHandler<UpdateStudentPasswordCommand>
{
    private readonly ILogger<UpdateStudentPasswordCommandHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public UpdateStudentPasswordCommandHandler(ILogger<UpdateStudentPasswordCommandHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        IUserAccountRepository userAccountRepository,
        IBCryptAuthProvider bCryptAuthProvider,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _userAccountRepository = userAccountRepository;
        _bCryptAuthProvider = bCryptAuthProvider;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(UpdateStudentPasswordCommand request, CancellationToken cancellationToken)
    {
        ErrorModel err;

        try
        {
            var studentInfo = await _studentInfoRepository.GetById(request.studentId, shouldTrack: true, cancellationToken: cancellationToken);

            if(studentInfo is null)
            {
                err = new(nameof(KeyNotFoundException), "Student not found");

                return ResultModel.Fail(err);
            }

            var newPasswordHash = _bCryptAuthProvider.EncryptPassword(request.newPassword);

            studentInfo.User.UserAccount.UpdatePassword(newPasswordHash, request.updatedBy);

            await _userAccountRepository.UpdateUserAccount(studentInfo.User.UserAccount, cancellationToken);

            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student password.");

            err = new(nameof(Exception), "Error updating student password.");

            return ResultModel.Fail(err);
        }
    }
}
