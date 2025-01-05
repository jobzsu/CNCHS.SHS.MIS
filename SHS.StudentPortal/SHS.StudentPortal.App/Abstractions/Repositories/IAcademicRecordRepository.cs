using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IAcademicRecordRepository
{
    public Task<AcademicRecord> CreateAcademicRecord(AcademicRecord academicRecord, CancellationToken cancellationToken = default);

    public Task<AcademicRecord> UpdateAcademicRecord(AcademicRecord academicRecord, CancellationToken cancellationToken = default);

    public Task<List<AcademicRecord>?> GetAcademicRecordsByStudentId(Guid studentId, CancellationToken cancellationToken = default, bool shouldTrack = false);
}
