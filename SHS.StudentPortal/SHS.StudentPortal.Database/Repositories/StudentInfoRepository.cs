using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class StudentInfoRepository : BaseRepository<StudentInfo>, IStudentInfoRepository
{
    private readonly AppDbContext _appDbContext;

    public StudentInfoRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<StudentInfo> Create(StudentInfo studentInfo, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(studentInfo, cancellationToken);
    }

    public async Task<StudentInfo> Update(StudentInfo studentInfo, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(studentInfo, cancellationToken);
    }

    public async Task Delete(StudentInfo studentInfo, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(studentInfo, cancellationToken);
    }

    public Task<StudentInfo?> GetById(Guid id, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            GetAll()
            .Include(x => x.User)
            .ThenInclude(x => x.UserAccount)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id) :
            GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .ThenInclude(x => x.UserAccount)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<StudentInfo>> GetListViaFilter(string? keyword,
        int yearLevel,
        Guid sectionId,
        string track,
        string strand,
        CancellationToken cancellationToken = default)
    {
        return await (keyword is null ?
            GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Section)
            .AsSplitQuery()
            .Where(x => (yearLevel != 0 ? x.YearLevel == yearLevel : true) &&
                        (sectionId != Guid.Empty ? x.SectionId == sectionId : true) &&
                        (!string.IsNullOrWhiteSpace(track) ? x.TrackAndStrand.Contains(track) : true) &&
                        (!string.IsNullOrWhiteSpace(strand) ? x.TrackAndStrand.Contains(strand) : true))
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Section)
            .AsSplitQuery()
            .Where(x => ((x.User.FirstName.ToLower().Contains(keyword)) ||
                        (x.User.LastName.ToLower().Contains(keyword)) ||
                        (x.User.MiddleName != null && x.User.MiddleName.ToLower().Contains(keyword))) &&
                        (yearLevel != 0 ? x.YearLevel == yearLevel : true) &&
                        (sectionId != Guid.Empty ? x.SectionId == sectionId : true) &&
                        (!string.IsNullOrWhiteSpace(track) ? x.TrackAndStrand.Contains(track) : true) &&
                        (!string.IsNullOrWhiteSpace(strand) ? x.TrackAndStrand.Contains(strand) : true))
            .ToListAsync(cancellationToken));
    }

    public async Task<List<StudentInfo>?> GetList(CancellationToken cancellationToken = default)
    {
        return await GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Section)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}
