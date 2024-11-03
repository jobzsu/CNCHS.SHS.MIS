using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>?> GetAllDepartments(bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Department>?> GetListViaFilter(string? searchKeyword,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);
}
