using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IDepartmentRepository
{
    Task<Department> CreateDepartment(Department department, CancellationToken cancellationToken = default);

    Task<Department> UpdateDepartment(Department department, CancellationToken cancellationToken = default);

    Task<List<Department>?> GetAllDepartments(bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Department>?> GetListViaFilter(string? searchKeyword,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);

    Task<Department?> GetDepartmentById(int id, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Department?> GetDepartmentByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default);
}
