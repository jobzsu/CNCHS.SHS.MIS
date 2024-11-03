using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;
using System.Linq;
using System.Linq.Expressions;

namespace SHS.StudentPortal.Database.Repositories;

public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Schedule> CreateSchedule(Schedule schedule, CancellationToken cancellationToken = default)
    {
        return await InsertAsync(schedule, cancellationToken);
    }

    public async Task<List<Schedule>?> GetListByDay(List<string> days, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Where(x => days.Count == 0 ? true :
                         ((days.Contains(SchoolDays.Monday.Id) ? x.Day.Contains(SchoolDays.Monday.Id) : false) ||
                         (days.Contains(SchoolDays.Tuesday.Id) ? x.Day.Contains(SchoolDays.Tuesday.Id) : false) ||
                         (days.Contains(SchoolDays.Wednesday.Id) ? x.Day.Contains(SchoolDays.Wednesday.Id) : false) ||
                         (days.Contains(SchoolDays.Thursday.Id) ? x.Day.Contains(SchoolDays.Thursday.Id) : false) ||
                         (days.Contains(SchoolDays.Friday.Id) ? x.Day.Contains(SchoolDays.Friday.Id) : false) ||
                         (days.Contains(SchoolDays.Saturday.Id) ? x.Day.Contains(SchoolDays.Saturday.Id) : false)))
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Where(x => days.Count == 0 ? true :
                         ((days.Contains(SchoolDays.Monday.Id) ? x.Day.Contains(SchoolDays.Monday.Id) : false) ||
                         (days.Contains(SchoolDays.Tuesday.Id) ? x.Day.Contains(SchoolDays.Tuesday.Id) : false) ||
                         (days.Contains(SchoolDays.Wednesday.Id) ? x.Day.Contains(SchoolDays.Wednesday.Id) : false) ||
                         (days.Contains(SchoolDays.Thursday.Id) ? x.Day.Contains(SchoolDays.Thursday.Id) : false) ||
                         (days.Contains(SchoolDays.Friday.Id) ? x.Day.Contains(SchoolDays.Friday.Id) : false) ||
                         (days.Contains(SchoolDays.Saturday.Id) ? x.Day.Contains(SchoolDays.Saturday.Id) : false)))
            .ToListAsync(cancellationToken));
    }

    public async Task<List<Schedule>?> GetListViaFilter(string? keyword, 
        List<string> days, 
        string? track,
        string? strand,
        string semester,
        string academicYear,
        CancellationToken cancellationToken = default)
    {
        return await (string.IsNullOrWhiteSpace(keyword) ?

            GetAll()
            .AsNoTracking()
            .Include(x => x.Instructor)
                .ThenInclude(x => x.User)
                    .ThenInclude(x => x.UserAccount)
            .Include(x => x.Subject)
            .AsSplitQuery()
            .Where(x => x.Subject.Semester.ToLower() == semester &&
                        x.Subject.AcademicYear.ToLower() == academicYear &&
                        (days.Count == 0 ? true : 
                         ((days.Contains(SchoolDays.Monday.Id) ? x.Day.Contains(SchoolDays.Monday.Id) : false) ||
                         (days.Contains(SchoolDays.Tuesday.Id) ? x.Day.Contains(SchoolDays.Tuesday.Id) : false) ||
                         (days.Contains(SchoolDays.Wednesday.Id) ? x.Day.Contains(SchoolDays.Wednesday.Id) : false) ||
                         (days.Contains(SchoolDays.Thursday.Id) ? x.Day.Contains(SchoolDays.Thursday.Id) : false) ||
                         (days.Contains(SchoolDays.Friday.Id) ? x.Day.Contains(SchoolDays.Friday.Id) : false) ||
                         (days.Contains(SchoolDays.Saturday.Id) ? x.Day.Contains(SchoolDays.Saturday.Id) : false))) &&
                        (!string.IsNullOrWhiteSpace(track) ? x.Subject.TrackAndStrand.Contains(track) : true) &&
                        (!string.IsNullOrWhiteSpace(strand) ? x.Subject.TrackAndStrand.Contains(strand) : true))
            .ToListAsync(cancellationToken) :

           GetAll()
            .AsNoTracking()
            .Include(x => x.Instructor)
                .ThenInclude(x => x.User)
                    .ThenInclude(x => x.UserAccount)
            .Include(x => x.Subject)
            .AsSplitQuery()
            .Where(x => ((x.RoomNumber.ToLower().Contains(keyword)) ||
                         (x.Instructor.User.FirstName.ToLower().Contains(keyword)) ||
                         (x.Instructor.User.LastName.ToLower().Contains(keyword)) ||
                         (x.Subject.Name.ToLower().Contains(keyword))) &&
                        x.Subject.Semester.ToLower() == semester &&
                        x.Subject.AcademicYear.ToLower() == academicYear &&
                        (days.Count == 0 ? true :
                         ((days.Contains(SchoolDays.Monday.Id) ? x.Day.Contains(SchoolDays.Monday.Id) : false) ||
                         (days.Contains(SchoolDays.Tuesday.Id) ? x.Day.Contains(SchoolDays.Tuesday.Id) : false) ||
                         (days.Contains(SchoolDays.Wednesday.Id) ? x.Day.Contains(SchoolDays.Wednesday.Id) : false) ||
                         (days.Contains(SchoolDays.Thursday.Id) ? x.Day.Contains(SchoolDays.Thursday.Id) : false) ||
                         (days.Contains(SchoolDays.Friday.Id) ? x.Day.Contains(SchoolDays.Friday.Id) : false) ||
                         (days.Contains(SchoolDays.Saturday.Id) ? x.Day.Contains(SchoolDays.Saturday.Id) : false))) &&
                        (!string.IsNullOrWhiteSpace(track) ? x.Subject.TrackAndStrand.Contains(track) : true) &&
                        (!string.IsNullOrWhiteSpace(strand) ? x.Subject.TrackAndStrand.Contains(strand) : true))
            .ToListAsync(cancellationToken));
    }

    public async Task<Schedule?> GetScheduleById(long id, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.Subject)
            .Include(x => x.Instructor)
            .ThenInclude(i => i.User)
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Include(x => x.Subject)
            .Include(x => x.Instructor)
            .ThenInclude(i => i.User)
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken));
    }

    public async Task<List<Schedule>?> GetSchedulesByInstructorIdSemesterAcademicYear(Guid instructorId, 
        string semester,
        string academicYear,
        bool shouldTrack = false, 
        CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.Subject)
            .Include(x => x.StudentSchedules)
            .ThenInclude(x => x.Student)
            .ThenInclude(x => x.AcademicRecords)
            .AsSplitQuery()
            .Where(x => x.InstructorId == instructorId &&
                        x.Subject.Semester == semester &&
                        x.Subject.AcademicYear == academicYear)
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Include(x => x.Subject)
            .Include(x => x.StudentSchedules)
            .ThenInclude(x => x.Student)
            .ThenInclude(x => x.AcademicRecords)
            .AsSplitQuery()
            .Where(x => x.InstructorId == instructorId &&
                        x.Subject.Semester == semester &&
                        x.Subject.AcademicYear == academicYear)
            .ToListAsync(cancellationToken));
    }

    public async Task<List<Schedule>?> GetSchedulesForStudentEnrollment(string track, 
        string strand, 
        string currentSemester, 
        string currentAcademicYear, 
        CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .Include(x => x.Subject)
            .Include(x => x.Instructor)
            .ThenInclude(i => i.User)
            .AsSplitQuery()
            .Where(x => (x.Subject.TrackAndStrand == $"{track}-{strand}") &&
                        (x.Subject.Semester == currentSemester) &&
                        (x.Subject.AcademicYear == currentAcademicYear))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Schedule>?> GetSelectedSchedules(List<long> scheduleIds, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .Include(x => x.Subject)
            .Include(x => x.Instructor)
            .ThenInclude(i => i.User)
            .AsSplitQuery()
            .Where(x => scheduleIds.Contains(x.Id))
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .Include(x => x.Subject)
            .Include(x => x.Instructor)
            .ThenInclude(i => i.User)
            .AsSplitQuery()
            .Where(x => scheduleIds.Contains(x.Id))
            .ToListAsync(cancellationToken));
    }

    public async Task<Schedule> UpdateSchedule(Schedule schedule, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(schedule, cancellationToken);
    }
}
