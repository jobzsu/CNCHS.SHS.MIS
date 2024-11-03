using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class MarkStudentForEnrollmentCommandHandler
    : ICommandHandler<MarkStudentForEnrollmentCommand>
{
    private readonly ILogger<MarkStudentForEnrollmentCommandHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public MarkStudentForEnrollmentCommandHandler(ILogger<MarkStudentForEnrollmentCommandHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(MarkStudentForEnrollmentCommand request, CancellationToken cancellationToken)
    {
        ErrorModel err;

        try
        {
            var studentInfo = await _studentInfoRepository.GetById(request.studentId, true, cancellationToken);

            if(studentInfo is null)
            {
                err = new(nameof(KeyNotFoundException), "Student not found.");

                return ResultModel.Fail(err);
            }

            studentInfo.UpdateStudentStatus(StudentStatuses.ForEnrollment.Id, request.verifiedBy);

            await _studentInfoRepository.Update(studentInfo, cancellationToken);
            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking student for enrollment.");

            err = new(nameof(Exception), "Error marking student for enrollment.");

            return ResultModel.Fail(err);
        }
    }
}
