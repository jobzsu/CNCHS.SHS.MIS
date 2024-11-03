using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class InstructorInfoRepository : BaseRepository<InstructorInfo>, IInstructorInfoRepository
{
    public InstructorInfoRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<InstructorInfo>?> GetList(bool includePlaceholder, 
        bool shouldTrack = false, 
        CancellationToken cancellation = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.User)
            .Where(x => (includePlaceholder ? true : (x.User.FirstName != Constants.NotApplicable)))
            .ToListAsync(cancellation) :
             GetAll()
             .AsNoTracking()
            .Include(x => x.User)
            .Where(x => (includePlaceholder ? true : (x.User.FirstName != Constants.NotApplicable)))
            .ToListAsync(cancellation));
    }

    public async Task<List<InstructorInfo>> GetListViaFilter(string? keyword, int departmentId, CancellationToken cancellationToken = default)
    {
        return await (string.IsNullOrWhiteSpace(keyword) ?
            GetAll()
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Department)
                .Where(x => (departmentId == 0 ? true : x.DepartmentId == departmentId))
                .AsSplitQuery()
                .ToListAsync(cancellationToken) :
            GetAll()
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Department)
                .Where(x => (departmentId == 0 ? true : x.DepartmentId == departmentId) &&
                            ((x.Department.Name.ToLower().Contains(keyword)) ||
                            (x.User.FirstName.ToLower().Contains(keyword)) ||
                            (x.User.MiddleName != null && x.User.MiddleName.ToLower().Contains(keyword))))
                .AsSplitQuery()
                .ToListAsync(cancellationToken));
    }
}
