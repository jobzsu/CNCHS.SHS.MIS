using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<Subject>?> GetAllSubjectBySemester(string semester, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Where(x => x.Semester.ToLower() == semester)
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Where(x => x.Semester.ToLower() == semester)
            .ToListAsync(cancellationToken));
    }

    public async Task<Subject?> GetSubjectById(int id, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) :
            GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken));
    }
}
