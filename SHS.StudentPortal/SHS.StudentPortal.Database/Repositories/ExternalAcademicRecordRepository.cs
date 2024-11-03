using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class ExternalAcademicRecordRepository : BaseRepository<ExternalAcademicRecord>, IExternalAcademicRecordRepository
{
    public ExternalAcademicRecordRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<ExternalAcademicRecord> CreateExternalAcademicRecord(ExternalAcademicRecord externalAcademicRecord, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(externalAcademicRecord, cancellationToken);
    }

    public async Task<List<ExternalAcademicRecord>?> GetExternalAcademicRecordsByStudentId(Guid studentId, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await GetAll()
            .AsNoTracking()
            .Where(x => x.StudentId == studentId)
            .ToListAsync(cancellationToken);
    }
}
