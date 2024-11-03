using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>?> GetAllDepartments(bool shouldTrack = false, CancellationToken cancellationToken = default);
}
