using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetInstructorViewModelHandler
    : IQueryHandler<GetInstructorViewModel, InstructorViewModel>
{
    private readonly ILogger<GetInstructorViewModelHandler> _logger;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISettingRepository _settingRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ISectionRepository _sectionRepository;

    public GetInstructorViewModelHandler(ILogger<GetInstructorViewModelHandler> logger,
        IInstructorInfoRepository instructorInfoRepository,
        IDepartmentRepository departmentRepository,
        ISettingRepository settingRepository,
        IScheduleRepository scheduleRepository,
        ISectionRepository sectionRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
        _departmentRepository = departmentRepository;
        _settingRepository = settingRepository;
        _scheduleRepository = scheduleRepository;
        _sectionRepository = sectionRepository;
    }

    public async Task<ResultModel<InstructorViewModel>> Handle(GetInstructorViewModel request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            if (currentSemester is null || currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Semester or Academic Year not found.");

                return ResultModel<InstructorViewModel>.Fail(error);
            }

            var instructorInfo = await _instructorInfoRepository
                .GetInstructorInfoById(request.instructorId,
                    cancellationToken: cancellationToken);

            if (instructorInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Instructor not found.");

                return ResultModel<InstructorViewModel>.Fail(error);
            }

            InstructorViewModel instructorViewModel = new()
            {
                Id = instructorInfo.Id,
                EmployeeId = instructorInfo.EmployeeId,
                Major = instructorInfo.Major,
                FirstName = instructorInfo.User.FirstName,
                MiddleName = instructorInfo.User.MiddleName ?? string.Empty,
                LastName = instructorInfo.User.LastName,
                ContactInfo = instructorInfo.ContactInformation,
                DepartmentId = instructorInfo.DepartmentId,
                DepartmentList = new(),
                AdvisorySectionId = instructorInfo.Section?.Id ?? Guid.Empty,
                Username = instructorInfo.User.UserAccount.Username,
                LastLogin = instructorInfo.User.UserAccount.LastLogin?.ToLocalTime().ToString() ?? "Never Logged In",
                SectionList = new(),
                Schedules = new(),
                CurrentSemester = currentSemester.Value,
                CurrentAcademicYear = currentAcademicYear.Value
            };

            var departmentList = await _departmentRepository.GetAllDepartments(cancellationToken: cancellationToken);
            
            if(departmentList is not null && departmentList.Count() > 0)
            {
                instructorViewModel.DepartmentList.AddRange(departmentList
                    .Select(x => new KeyValuePair<int, string>(x.Id, x.Name)));
            }

            var sectionList = await _sectionRepository.GetAllSections(includeNotApplicable: true, cancellationToken: cancellationToken);

            if(sectionList is not null && sectionList.Count() > 0)
            {
                instructorViewModel.SectionList.AddRange(sectionList
                    .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name)));
            }

            var instructorSchedules = await _scheduleRepository
                .GetSchedulesByInstructorIdSemesterAcademicYear(request.instructorId,
                    currentSemester.Value,
                    currentAcademicYear.Value,
                    cancellationToken: cancellationToken);

            if(instructorSchedules is not null && instructorSchedules.Count() > 0)
            {
                foreach(var sched in instructorSchedules)
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

                    instructorViewModel.Schedules.Add(new()
                    {
                        Subject = sched.Subject.Name,
                        Days = string.Join(",", days),
                        Time = $"{timeStartStr} - {timeEndStr}",
                        RoomNumber = sched.RoomNumber,
                        GradesSubmitted = gradesSubmitted ? "YES" : "NO"
                    });
                }
            }

            return ResultModel<InstructorViewModel>.Success(instructorViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Getting Instructor Details");

            error = new(nameof(KeyNotFoundException), "Error Getting Instructor Details");

            return ResultModel<InstructorViewModel>.Fail(error);
        }
    }
}
