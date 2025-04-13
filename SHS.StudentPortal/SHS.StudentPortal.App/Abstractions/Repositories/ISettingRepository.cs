using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface ISettingRepository
{
    Task<Setting?> GetSettingByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Setting> UpdateSystemSetting(Setting setting, CancellationToken cancellationToken = default);
}
