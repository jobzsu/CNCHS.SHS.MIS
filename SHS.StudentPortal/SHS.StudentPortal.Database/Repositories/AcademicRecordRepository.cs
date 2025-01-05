using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class AcademicRecordRepository : BaseRepository<AcademicRecord>, IAcademicRecordRepository
{
    private readonly AppDbContext _appDbContext;

    public AcademicRecordRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<AcademicRecord> CreateAcademicRecord(AcademicRecord academicRecord, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(academicRecord, cancellationToken);
    }

    public async Task<List<AcademicRecord>?> GetAcademicRecordsByStudentId(Guid studentId, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return await (shouldTrack ?
            GetAll()
                .AsSplitQuery()
                .Where(x => x.StudentId == studentId)
                .ToListAsync(cancellationToken) :
            GetAll()
                .AsSplitQuery()
                .AsNoTracking()
                .Where(x => x.StudentId == studentId)
                .ToListAsync(cancellationToken));
    }

    public async Task<AcademicRecord> UpdateAcademicRecord(AcademicRecord academicRecord, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(academicRecord, cancellationToken);
    }
}
