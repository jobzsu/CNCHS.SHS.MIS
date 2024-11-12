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
            await GetAll().Where(s => includeNotApplicable ? true : s.Name != Constants.NotApplicable).ToListAsync(cancellationToken) :
            await GetAll().Where(s => includeNotApplicable ? true : s.Name != Constants.NotApplicable).AsNoTracking().ToListAsync(cancellationToken);
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
