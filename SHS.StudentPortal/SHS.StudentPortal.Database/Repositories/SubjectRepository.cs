using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Subject> UpdateSubject(Subject subject, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(subject, cancellationToken);
    }

    public async Task<List<Subject>?> GetAllSubjectBySemester(string semester, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Where(x => x.Semester.ToLower() == semester && x.Code != "000000")
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Where(x => x.Semester.ToLower() == semester && x.Code != "000000")
            .ToListAsync(cancellationToken));
    }

    public async Task<List<Subject>?> GetAllSubjects(bool includeOther = false, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return await (shouldTrack ?
            GetAll()
            .Where(x => includeOther ? true : x.Code != "000000")
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Where(x => includeOther ? true : x.Code != "000000")
            .ToListAsync(cancellationToken));
    }

    public async Task<List<Subject>?> GetListViaFilter(string? keyword, string? track, string? strand, CancellationToken cancellationToken = default)
    {
        return string.IsNullOrWhiteSpace(keyword) ?
            await GetAll()
            .AsNoTracking()
            .Where(x => (!string.IsNullOrWhiteSpace(track) ? x.TrackAndStrand.Contains(track) : true) && 
                        (!string.IsNullOrWhiteSpace(strand) ? x.TrackAndStrand.Contains(strand) : true) && x.Code != "000000")
            .ToListAsync(cancellationToken) :
            await GetAll()
            .AsNoTracking()
            .Where(x => (!string.IsNullOrWhiteSpace(track) ? x.TrackAndStrand.Contains(track) : true) &&
                        (!string.IsNullOrWhiteSpace(strand) ? x.TrackAndStrand.Contains(strand) : true) &&
                        (x.Name.ToLower().Contains(keyword)) ||
                        (x.Code.ToLower().Contains(keyword)) && x.Code != "000000")
            .ToListAsync(cancellationToken);
    }

    public Task<Subject?> GetSubjectByCode(string code, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            GetAll()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken) :
            GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
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

    public Task<Subject?> GetSubjectByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            GetAll()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name, cancellationToken) :
            GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name, cancellationToken);
    }

    public async Task<List<Subject>?> GetAllSubjectByIdList(List<int> subjectIds, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
                    .Where(s => subjectIds.Contains(s.Id))
                    .ToListAsync(cancellationToken) :
             await GetAll()
                    .AsNoTracking()
                    .Where(s => subjectIds.Contains(s.Id))
                    .ToListAsync(cancellationToken);
    }
}
