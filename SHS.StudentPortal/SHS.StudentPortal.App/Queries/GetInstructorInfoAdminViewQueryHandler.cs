using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

public sealed class GetInstructorInfoAdminViewQueryHandler
    : IQueryHandler<GetInstructorInfoAdminViewQuery, InstructorInfoAdminViewDTO>
{
    private readonly ILogger<GetInstructorInfoAdminViewQueryHandler> _logger;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly ISettingRepository _settingRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public GetInstructorInfoAdminViewQueryHandler(ILogger<GetInstructorInfoAdminViewQueryHandler> logger,
        IInstructorInfoRepository instructorInfoRepository,
        ISettingRepository settingRepository,
        ISectionRepository sectionRepository,
        IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
        _settingRepository = settingRepository;
        _sectionRepository = sectionRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<ResultModel<InstructorInfoAdminViewDTO>> Handle(GetInstructorInfoAdminViewQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            if (currentSemester is null || currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Semester or Academic Year not found.");

                return ResultModel<InstructorInfoAdminViewDTO>.Fail(error);
            }

            var instructorInfo = await _instructorInfoRepository.GetInstructorInfoById(request.instructorId, cancellationToken: cancellationToken);

            if (instructorInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Instructor not found.");

                return ResultModel<InstructorInfoAdminViewDTO>.Fail(error);
            }

            var advisorySection = await _sectionRepository.GetByAdviserId(instructorInfo.Id, cancellationToken: cancellationToken);
            var notApplicableSection = await _sectionRepository.GetSectionByName(Constants.NotApplicable, cancellationToken: cancellationToken);

            InstructorInfoAdminViewDTO retVal = new()
            {
                Id = instructorInfo.Id,
                EmployeeId = instructorInfo.EmployeeId,
                Major = instructorInfo.Major,
                FirstName = instructorInfo.User.FirstName,
                MiddleName = instructorInfo.User.MiddleName ?? string.Empty,
                LastName = instructorInfo.User.LastName,
                ContactInfo = instructorInfo.ContactInformation,
                DepartmentId = instructorInfo.DepartmentId,
                AdvisorySectionId = advisorySection?.Id ?? notApplicableSection!.Id,
                Username = instructorInfo.User.UserAccount.Username,
                LastLogin = instructorInfo.User.UserAccount.LastLogin?.ToLocalTime() ?? null,
                Schedules = new(),
            };

            var instructorSchedules = await _scheduleRepository
                .GetSchedulesByInstructorIdSemesterAcademicYear(request.instructorId,
                    currentSemester.Value,
                    currentAcademicYear.Value,
                    cancellationToken: cancellationToken);

            if (instructorSchedules is not null && instructorSchedules.Any())
            {
                retVal.Schedules.AddRange(instructorSchedules.Select(sched =>
                {
                    var timeStartMin = sched.TimeStart.Minute.ToString().PadLeft(2, '0');

                    var timeStartStr = sched.TimeStart.Hour < 12 ?
                        $"{sched.TimeStart.Hour}:{timeStartMin} AM" :
                        (sched.TimeStart.Hour == 12 ? $"{sched.TimeStart.Hour}:{timeStartMin} PM" : $"{sched.TimeStart.Hour - 12}:{timeStartMin} PM");

                    var timeEndMin = sched.TimeEnd.Minute.ToString().PadLeft(2, '0');

                    var timeEndStr = sched.TimeEnd.Hour < 12 ?
                        $"{sched.TimeEnd.Hour}:{timeEndMin} AM" :
                        (sched.TimeEnd.Hour == 12 ? $"{sched.TimeEnd.Hour}:{timeEndMin} PM" : $"{sched.TimeEnd.Hour - 12}:{timeEndMin} PM");

                    var daysArr = sched.Day.Split(',').ToList();
                    var days = new List<string>();

                    foreach (var d in daysArr)
                    {
                        days.Add(SchoolDays.Get(d).Name);
                    }

                    // Check if all students for this schedule has grades
                    var gradesSubmitted = sched.StudentSchedules.Any() && sched.StudentSchedules.All(x =>
                        x.Student.AcademicRecords
                            .Any(ar => ar.Semester == currentSemester.Value &&
                                                  ar.AcademicYear == currentAcademicYear.Value &&
                                                  ar.SubjectId == sched.SubjectId));

                    return new Schedules()
                    {
                        Subject = sched.Subject.Name,
                        Days = string.Join(",", days),
                        Time = $"{timeStartStr} - {timeEndStr}",
                        RoomNumber = sched.RoomNumber,
                        GradesSubmitted = gradesSubmitted ? "YES" : "NO"
                    };
                }));
            }

            return ResultModel<InstructorInfoAdminViewDTO>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Instructor Info");

            error = new(nameof(Exception), "An error occurred while retrieving instructor info.");

            return ResultModel<InstructorInfoAdminViewDTO>.Fail(error);
        }
    }
}
