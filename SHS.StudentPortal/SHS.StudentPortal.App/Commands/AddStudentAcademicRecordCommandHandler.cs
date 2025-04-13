using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class AddStudentAcademicRecordCommandHandler
    : ICommandHandler<AddStudentAcademicRecordCommand, AcademicRecordAdminViewDTO>
{
    private readonly ILogger<AddStudentAcademicRecordCommandHandler> _logger;
    private readonly IBaseUnitOfWork _baseUnitOfWork;
    private readonly IAcademicRecordRepository _academicRecordRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IUserRepository _userRepository;

    public AddStudentAcademicRecordCommandHandler(ILogger<AddStudentAcademicRecordCommandHandler> logger,
        IBaseUnitOfWork baseUnitOfWork,
        IAcademicRecordRepository academicRecordRepository,
        ISubjectRepository subjectRepository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _baseUnitOfWork = baseUnitOfWork;
        _academicRecordRepository = academicRecordRepository;
        _subjectRepository = subjectRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultModel<AcademicRecordAdminViewDTO>> Handle(AddStudentAcademicRecordCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var newAcademicRecord = new AcademicRecord().Create(
                request.academicRecord.AcademicYear,
                request.academicRecord.Semester,
                request.academicRecord.SubjectId,
                request.academicRecord.OtherSubjectName,
                request.academicRecord.Rating,
                request.academicRecord.StudentId,
                request.academicRecord.CreatedById,
                request.academicRecord.CreatedById);

            using (var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    await _academicRecordRepository.CreateAcademicRecord(newAcademicRecord, cancellationToken);

                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }

            var subject = await _subjectRepository.GetSubjectById(newAcademicRecord.SubjectId, cancellationToken: cancellationToken);

            var encodedBy = await _userRepository.GetUserByUserAccountId(newAcademicRecord.EncodedById!.Value, cancellationToken);

            var retVal = new AcademicRecordAdminViewDTO()
            {
                Id = newAcademicRecord.Id,
                Rating = newAcademicRecord.Rating,
                Semester = newAcademicRecord.Semester,
                SubjectId = newAcademicRecord.SubjectId,
                SubjectName = newAcademicRecord.SubjectId == 0 ? newAcademicRecord.OtherSubjectName : subject!.Name,
                AcademicYear = newAcademicRecord.AcademicYear,
                EncodedBy = $"{encodedBy!.FirstName} {encodedBy!.LastName}",
                EncodedById = newAcademicRecord.EncodedById!.Value,
                VerifiedBy = "N/A",
                VerifiedById = null,
                VerifiedDate = null,
                TempId = 0
            };

            return ResultModel<AcademicRecordAdminViewDTO>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding student academic record");

            error = new(nameof(Exception), "Error adding student academic record");

            return ResultModel<AcademicRecordAdminViewDTO>.Fail(error);
        }
    }
}
