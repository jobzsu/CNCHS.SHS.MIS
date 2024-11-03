using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IScheduleRepository
{
    Task<Schedule> CreateSchedule(Schedule schedule, CancellationToken cancellationToken = default);

    Task<Schedule?> GetScheduleById(long id, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Schedule> UpdateSchedule(Schedule schedule, CancellationToken cancellationToken = default);

    Task<List<Schedule>?> GetListViaFilter(string? keyword,
        List<string> days,
        string? track,
        string? strand,
        string semester,
        string academicYear,
        CancellationToken cancellationToken = default);

    Task<List<Schedule>?> GetListByDay(List<string> days, bool shouldTrack = false, CancellationToken cancellationToken = default);
    
    Task<List<Schedule>?> GetSchedulesForStudentEnrollment(string track, string strand, string currentSemester, string currentAcademicYear, CancellationToken cancellationToken);

    Task<List<Schedule>?> GetSelectedSchedules(List<long> scheduleIds, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Schedule>?> GetSchedulesByInstructorIdSemesterAcademicYear(Guid instructorId,
        string currentSemester,
        string currentAcademicYear,
        bool shouldTrack = false, 
        CancellationToken cancellationToken = default);
}
