using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IExternalAcademicRecordRepository
{
    Task<ExternalAcademicRecord> CreateExternalAcademicRecord(ExternalAcademicRecord externalAcademicRecord,
        CancellationToken cancellationToken = default);

    Task<List<ExternalAcademicRecord>?> GetExternalAcademicRecordsByStudentId(Guid studentId,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);
}
