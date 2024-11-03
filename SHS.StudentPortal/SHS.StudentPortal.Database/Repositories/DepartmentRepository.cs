using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<Department>?> GetAllDepartments(bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().ToListAsync(cancellationToken) :
            await GetAll().AsNoTracking().ToListAsync(cancellationToken);
    }
}
