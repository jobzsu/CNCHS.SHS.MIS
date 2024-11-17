using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Department> CreateDepartment(Department department, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(department, cancellationToken);
    }

    public async Task<List<Department>?> GetAllDepartments(bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().Where(d => d.Name != Constants.NotApplicable).ToListAsync(cancellationToken) :
            await GetAll().Where(d => d.Name != Constants.NotApplicable).AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<Department?> GetDepartmentById(int id, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            GetAll().FirstOrDefaultAsync(x => x.Id == id, cancellationToken) :
            GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Department?> GetDepartmentByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            GetAll().FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken) :
            GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<List<Department>?> GetListViaFilter(string? searchKeyword, 
        bool shouldTrack = false, 
        CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.Instructors)
            .AsSplitQuery()
            .Where(x => string.IsNullOrWhiteSpace(searchKeyword) ?
                true : x.Name.ToLower().Contains(searchKeyword))
            .ToListAsync(cancellationToken) :
            GetAll()
            .Include(x => x.Instructors)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(x => string.IsNullOrWhiteSpace(searchKeyword) ?
                true : x.Name.ToLower().Contains(searchKeyword))
            .ToListAsync(cancellationToken));
    }
}
