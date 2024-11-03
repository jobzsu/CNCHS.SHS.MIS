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

    public GetInstructorViewModelHandler(ILogger<GetInstructorViewModelHandler> logger,
        IInstructorInfoRepository instructorInfoRepository,
        IDepartmentRepository departmentRepository,
        ISettingRepository settingRepository,
        IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
        _departmentRepository = departmentRepository;
        _settingRepository = settingRepository;
        _scheduleRepository = scheduleRepository;
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
                    currentSemester.Value,
                    currentAcademicYear.Value,
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
                AdvisorySection = instructorInfo.Section.Name,
                Schedules = new()
            };

            var departmentList = await _departmentRepository.GetAllDepartments(cancellationToken: cancellationToken);
            
            if(departmentList is not null && departmentList.Count() > 0)
            {
                instructorViewModel.DepartmentList.AddRange(departmentList
                    .Select(x => new KeyValuePair<int, string>(x.Id, x.Name)));
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
                    var timeStartStr = sched.TimeStart.Hour < 12 ?
                        $"{sched.TimeStart.Hour}:{sched.TimeStart.Minute} AM" :
                        (sched.TimeStart.Hour == 12 ? $"{sched.TimeStart.Hour}:{sched.TimeStart.Minute} PM" : $"{sched.TimeStart.Hour - 12}:{sched.TimeStart.Minute} PM");

                    var timeEndStr = sched.TimeEnd.Hour < 12 ?
                        $"{sched.TimeEnd.Hour}:{sched.TimeEnd.Minute} AM" :
                        (sched.TimeEnd.Hour == 12 ? $"{sched.TimeEnd.Hour}:{sched.TimeEnd.Minute} PM" : $"{sched.TimeEnd.Hour - 12}:{sched.TimeEnd.Minute} PM");

                    var daysArr = sched.Day.Split(',').ToList();
                    var days = new List<string>();

                    foreach (var d in daysArr)
                    {
                        days.Add(SchoolDays.Get(d).Name);
                    }

                    // Check if all students for this schedule has grades
                    var gradesSubmitted = sched.StudentSchedules.All(x =>
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
