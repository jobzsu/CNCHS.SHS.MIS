using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class ApproveStudentCommandHandler
    : ICommandHandler<ApproveStudentCommand>
{
    private readonly ILogger<ApproveStudentCommandHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public ApproveStudentCommandHandler(ILogger<ApproveStudentCommandHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        IUserAccountRepository userAccountRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _userAccountRepository = userAccountRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(ApproveStudentCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var studentInfo = await _studentInfoRepository
                .GetById(request.studentId, true, cancellationToken);

            if (studentInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Student Information not found.");

                return ResultModel.Fail(error);
            }

            studentInfo.UpdateStudentStatus(StudentStatuses.ForAssessment.Id, request.approvedById);

            var studentUserAccount = await _userAccountRepository
                        .GetUserAccountById(studentInfo.User.UserAccountId, cancellationToken, true);

            if (studentUserAccount is null)
            {
                error = new(nameof(KeyNotFoundException), "Student User Account not found.");

                return ResultModel.Fail(error);
            }

            studentUserAccount.SetToActive(request.approvedById);

            using (var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    await _studentInfoRepository.Update(studentInfo, cancellationToken);

                    await _userAccountRepository.UpdateUserAccount(studentUserAccount, cancellationToken);

                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while approving student. {ex.Message}");

            error = new(nameof(ex), "Error occurred while approving student.");

            return ResultModel.Fail(error);
        }
    }
}
