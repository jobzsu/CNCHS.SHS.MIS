using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IInstructorInfoRepository
{
    Task<List<InstructorInfo>> GetListViaFilter(string? keyword,
        int departmentId,
        CancellationToken cancellationToken = default);

    Task<List<InstructorInfo>?> GetList(bool includePlaceholder,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);
}
