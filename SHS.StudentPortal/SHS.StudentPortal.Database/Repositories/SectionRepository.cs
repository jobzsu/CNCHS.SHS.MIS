using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    private readonly AppDbContext _appDbContext;

    public SectionRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Section>?> GetAllSections(bool includeNotApplicable = false, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().Where(s => includeNotApplicable ? true : s.Name != Constants.NotApplicable)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken) :
            await GetAll().Where(s => includeNotApplicable ? true : s.Name != Constants.NotApplicable)
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
    }

    public async Task<List<Section>?> GetAvailableSections(Guid instructorId, 
        bool shouldTrack = false, 
        CancellationToken cancellationToken = default)
    {
        var notApplicableInstructorInfoId = await GetAll()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == Constants.NotApplicable.ToLower(), cancellationToken);

        return shouldTrack ?
            await GetAll()
                .Where(s => s.Name == Constants.NotApplicable || 
                            s.AdviserId == notApplicableInstructorInfoId!.AdviserId ||
                            s.AdviserId == instructorId)
                .ToListAsync(cancellationToken) :
            await GetAll()
                .Where(s => s.Name == Constants.NotApplicable ||
                            s.AdviserId == notApplicableInstructorInfoId!.AdviserId ||
                            s.AdviserId == instructorId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }

    public async Task<Section?> GetByAdviserId(Guid adviserId, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().FirstOrDefaultAsync(s => s.AdviserId == adviserId, cancellationToken) :
            await GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.AdviserId == adviserId, cancellationToken);
    }

    public Task<Section?> GetById(Guid id, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            GetAll().FirstOrDefaultAsync(x => x.Id == id, cancellationToken) :
            GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Section?> GetSectionByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken) :
            await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<Section> UpdateSection(Section section, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(section, cancellationToken);
    }
}
