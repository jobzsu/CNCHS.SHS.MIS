using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class SettingRepository : BaseRepository<Setting>, ISettingRepository
{
    public SettingRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<Setting?> GetSettingByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name, cancellationToken) :
            GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name, cancellationToken));
    }

    public async Task<Setting> UpdateSystemSetting(Setting setting, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(setting, cancellationToken);
    }
}
