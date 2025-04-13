using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetAvailableScheduleForStudentEnrollmentQueryHandler
    : IQueryHandler<GetAvailableScheduleForStudentEnrollmentQuery, List<EnrollStudentScheduleListViewModel>>
{
    private readonly ILogger<GetAvailableScheduleForStudentEnrollmentQueryHandler> _logger;
    private readonly IBaseUnitOfWork _unitOfWork;
    private readonly ISettingRepository _settingRepository;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IPreRequisiteRepository _preRequisiteRepository;
    private readonly ISubjectRepository _subjectRepository;

    public GetAvailableScheduleForStudentEnrollmentQueryHandler(ILogger<GetAvailableScheduleForStudentEnrollmentQueryHandler> logger,
        IBaseUnitOfWork unitOfWork,
        ISettingRepository settingRepository,
        IStudentInfoRepository studentInfoRepository,
        IScheduleRepository scheduleRepository,
        IPreRequisiteRepository preRequisiteRepository,
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _settingRepository = settingRepository;
        _studentInfoRepository = studentInfoRepository;
        _scheduleRepository = scheduleRepository;
        _preRequisiteRepository = preRequisiteRepository;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel<List<EnrollStudentScheduleListViewModel>>> Handle(GetAvailableScheduleForStudentEnrollmentQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            if (currentSemester is null || currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Semester or Academic Year not found.");
                return ResultModel<List<EnrollStudentScheduleListViewModel>>.Fail(error);
            }

            var studentInfo = await _studentInfoRepository.GetById(request.studentId, cancellationToken: cancellationToken);

            if (studentInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Student not found.");
                return ResultModel<List<EnrollStudentScheduleListViewModel>>.Fail(error);
            }

            var track = Track.GetTrack(studentInfo.TrackAndStrand.Split('-')[0]);
            var strand = Strand.GetStrand(studentInfo.TrackAndStrand.Split('-')[1]);

            var availableSchedules = await _scheduleRepository.GetSchedulesForStudentEnrollment(track.Id,
                strand.Id,
                currentSemester.Value,
                currentAcademicYear.Value,
                cancellationToken);

            List<EnrollStudentScheduleListViewModel> retVal = new();

            if(availableSchedules is not null && availableSchedules.Any())
            {
                var subjectIds = availableSchedules.Select(s => s.SubjectId).ToList();

                var preRequisitesList = await _preRequisiteRepository.GetAllListForSubjects(subjectIds, cancellationToken);

                var preRequisitesSubjectIdList = preRequisitesList?.Select(p => p.PreRequisiteSubjectId).ToList() ?? new List<int>();

                var preRequisitesSubjectList = await _subjectRepository.GetAllSubjectByIdList(preRequisitesSubjectIdList, cancellationToken);

                retVal.AddRange(availableSchedules.Select(s =>
                {
                    List<string> days = new();
                    foreach (var day in s.Day.Split(','))
                        days.Add(SchoolDays.Get(day).Name);

                    var timeStartStr = s.TimeStart.Hour < 12 ?
                    $"{s.TimeStart.Hour}:{s.TimeStart.Minute} AM" :
                    (s.TimeStart.Hour == 12 ? $"{s.TimeStart.Hour}:{s.TimeStart.Minute} PM" : $"{s.TimeStart.Hour - 12}:{s.TimeStart.Minute} PM");

                    var timeEndStr = s.TimeEnd.Hour < 12 ?
                        $"{s.TimeEnd.Hour}:{s.TimeEnd.Minute} AM" :
                        (s.TimeEnd.Hour == 12 ? $"{s.TimeEnd.Hour}:{s.TimeEnd.Minute} PM" : $"{s.TimeEnd.Hour - 12}:{s.TimeEnd.Minute} PM");

                    var preRequisites = preRequisitesList?.Where(p => p.SubjectId == s.SubjectId).ToList();
                    var preRequisiteSubjNames = new List<string>();

                    if (preRequisites is not null && preRequisites.Any())
                    {
                        preRequisiteSubjNames.AddRange(preRequisites.Select(pr => pr.Subject.Name));
                    }

                    return new EnrollStudentScheduleListViewModel
                    {
                        ScheduleId = s.Id,
                        Subject = $"[{s.Subject.Code}] {s.Subject.Name}",
                        Instructor = s.Instructor.User.MiddleName is null ?
                                $"Prof. {s.Instructor.User.FirstName} {s.Instructor.User.LastName}" :
                                $"Prof. {s.Instructor.User.FirstName} {s.Instructor.User.MiddleName} {s.Instructor.User.LastName}",
                        Days = string.Join(',', days),
                        Time = $"{timeStartStr} - {timeEndStr}",
                        Room = s.RoomNumber,
                        PreReqSubjects = (preRequisiteSubjNames is not null && !preRequisiteSubjNames.Any()) ? preRequisiteSubjNames : new(),
                        IsSelected = false
                    };
                }));
            }

            return ResultModel<List<EnrollStudentScheduleListViewModel>>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available schedule for student enrollment");

            error = new(nameof(Exception), "Error getting available schedule for student enrollment");

            return ResultModel<List<EnrollStudentScheduleListViewModel>>.Fail(error);
        }
    }
}
