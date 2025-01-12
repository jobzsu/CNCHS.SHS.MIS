using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class EncodeStudentGradesCommandHandler
    : ICommandHandler<EncodeStudentGradesCommand>
{
    private readonly ILogger<EncodeStudentGradesCommandHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IAcademicRecordRepository _academicRecordRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public EncodeStudentGradesCommandHandler(ILogger<EncodeStudentGradesCommandHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        IAcademicRecordRepository academicRecordRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _academicRecordRepository = academicRecordRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(EncodeStudentGradesCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var student = await _studentInfoRepository.GetById(request.studentId, false, cancellationToken);

            if(student is null)
            {
                error = new ErrorModel(nameof(ArgumentNullException), "Student not found");

                return ResultModel.Fail(error);
            }

            if(!request.academicRecords.Any())
            {
                error = new(nameof(ArgumentException), "No Academic Records to save");

                return ResultModel.Fail(error);
            }

            using (var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    foreach(var ar in request.academicRecords)
                    {
                        var newAcademicRecord = new AcademicRecord()
                            .Create(ar.AcademicYear, ar.Semester, int.Parse(ar.SubjectId), ar.OtherSubjName,
                                ar.Rating, request.studentId, request.encodedById, request.encodedById);

                        await _academicRecordRepository.CreateAcademicRecord(newAcademicRecord, cancellationToken);

                        await _baseUnitOfWork.SaveChangesAsync(cancellationToken);
                    }

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
            _logger.LogError($"Error while trying to save Academic Records: {ex.Message}");

            error = new ErrorModel(nameof(Exception), "An error while trying to save Academic Records");

            return ResultModel.Fail(error);
        }
    }
}
