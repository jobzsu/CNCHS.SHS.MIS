using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class InstructorInfoRepository : BaseRepository<InstructorInfo>, IInstructorInfoRepository
{
    public InstructorInfoRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<InstructorInfo> CreateInstructorInfo(InstructorInfo instructorInfo, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(instructorInfo, cancellationToken);
    }

    public async Task<InstructorInfo?> GetByUserId(Guid userId, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) :
            await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task<InstructorInfo?> GetInstructorInfoByEmployeeId(string employeeId, bool shouldTrack = false, CancellationToken cancellation = default)
    {
        return await (shouldTrack ?
            GetAll()
              .FirstOrDefaultAsync(i => i.EmployeeId.ToLower() == employeeId, cancellation) :
            GetAll()
              .AsNoTracking()
              .FirstOrDefaultAsync(i => i.EmployeeId.ToLower() == employeeId, cancellation));
    }

    public async Task<InstructorInfo?> GetInstructorInfoById(Guid instructorId,
        bool shouldTrack = false, 
        CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.User)
            .ThenInclude(u => u.UserAccount)
            .Include(x => x.Department)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == instructorId, cancellationToken) :
            GetAll()
            .Include(x => x.User)
            .ThenInclude(u => u.UserAccount)
            .Include(x => x.Department)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == instructorId, cancellationToken));
    }

    public async Task<List<InstructorInfo>?> GetInstructorsByDepartment(int departmentId, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll()
            .Include(x => x.User)
            .Where(x => x.DepartmentId == departmentId)
            .ToListAsync(cancellationToken) :
            await GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.DepartmentId == departmentId)
            .ToListAsync(cancellationToken);
    }

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
                .Where(x => (departmentId == 0 ? x.EmployeeId != "4000": x.DepartmentId == departmentId && x.EmployeeId != "4000"))
                .AsSplitQuery()
                .ToListAsync(cancellationToken) :
            GetAll()
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Department)
                .Where(x => (departmentId == 0 ? x.EmployeeId != "4000" : x.DepartmentId == departmentId && x.EmployeeId != "4000") &&
                            ((x.Department.Name.ToLower().Contains(keyword)) ||
                            (x.User.FirstName.ToLower().Contains(keyword)) ||
                            (x.User.MiddleName != null && x.User.MiddleName.ToLower().Contains(keyword))))
                .AsSplitQuery()
                .ToListAsync(cancellationToken));
    }

    public async Task<InstructorInfo> UpdateInstructorInfo(InstructorInfo instructorInfo, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(instructorInfo, cancellationToken);
    }
}
