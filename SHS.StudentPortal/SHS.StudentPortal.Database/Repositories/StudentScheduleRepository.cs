using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class StudentScheduleRepository : BaseRepository<StudentSchedule>, IStudentScheduleRepository
{
    public StudentScheduleRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<StudentSchedule> CreateStudentSchedule(StudentSchedule studentSchedule, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(studentSchedule, cancellationToken);
    }

    public async Task<List<StudentSchedule>?> GetByStudentIdCurrentSemesterAcademicYear(Guid studentId, 
        string currentSemester, 
        string currentAcademicYear, 
        bool shouldTrack = false,
        CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.Schedule)
            .ThenInclude(x => x.Instructor)
            .ThenInclude(x => x.User)
            .Include(x => x.Schedule)
            .ThenInclude(x => x.Subject)
            .AsSplitQuery()
            .Where(x => x.StudentId == studentId && 
                        x.Semester.ToLower() == currentSemester.ToLower() && 
                        x.AcademicYear.ToLower() == currentAcademicYear.ToLower())
            .ToListAsync(cancellationToken) :
            GetAll()
            .Include(x => x.Schedule)
            .ThenInclude(x => x.Instructor)
            .ThenInclude(x => x.User)
            .Include(x => x.Schedule)
            .ThenInclude(x => x.Subject)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(x => x.StudentId == studentId && 
                        x.Semester.ToLower() == currentSemester.ToLower() && 
                        x.AcademicYear.ToLower() == currentAcademicYear.ToLower())
            .ToListAsync(cancellationToken));
    }
}
