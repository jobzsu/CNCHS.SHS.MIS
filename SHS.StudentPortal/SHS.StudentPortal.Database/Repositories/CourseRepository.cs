using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;
using System.Threading;

namespace SHS.StudentPortal.Database.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    private readonly AppDbContext _appDbContext;

    public CourseRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Course>?> GetAllCourses(bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().ToListAsync(cancellationToken) :
            await GetAll().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByName(string courseName, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return shouldTrack ?
            await GetAll().FirstOrDefaultAsync(x => x.Name.ToLower() == courseName.ToLower(), cancellationToken) :
            await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == courseName.ToLower(), cancellationToken);
    }
}
