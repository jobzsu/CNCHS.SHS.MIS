using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetStudentEnrollmentViewModelHandler
    : IQueryHandler<GetStudentEnrollmentViewModelQuery, EnrollStudentViewModel>
{
    private readonly ILogger<GetStudentEnrollmentViewModelHandler> _logger;
    private readonly ISettingRepository _settingRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IPreRequisiteRepository _preRequisiteRepository;

    public GetStudentEnrollmentViewModelHandler(ILogger<GetStudentEnrollmentViewModelHandler> logger,
        ISettingRepository settingRepository,
        IScheduleRepository scheduleRepository,
        IStudentInfoRepository studentInfoRepository,
        ISectionRepository sectionRepository,
        ISubjectRepository subjectRepository,
        IPreRequisiteRepository preRequisiteRepository)
    {
        _logger = logger;
        _settingRepository = settingRepository;
        _scheduleRepository = scheduleRepository;
        _studentInfoRepository = studentInfoRepository;
        _sectionRepository = sectionRepository;
        _subjectRepository = subjectRepository;
        _preRequisiteRepository = preRequisiteRepository;
    }

    public async Task<ResultModel<EnrollStudentViewModel>> Handle(GetStudentEnrollmentViewModelQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);
            
            if(currentSemester is null || currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Semester or Academic Year not found.");
                return ResultModel<EnrollStudentViewModel>.Fail(error);
            }

            var studentInfo = await _studentInfoRepository.GetById(request.studentId, cancellationToken: cancellationToken);

            if(studentInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Student not found.");
                return ResultModel<EnrollStudentViewModel>.Fail(error);
            }

            var track = Track.GetTrack(studentInfo.TrackAndStrand.Split('-')[0]);
            var strand = Strand.GetStrand(studentInfo.TrackAndStrand.Split('-')[1]);

            var sectionList = await _sectionRepository.GetAllSections(includeNotApplicable: true, cancellationToken: cancellationToken);

            var scheduleList = await _scheduleRepository
                .GetSchedulesForStudentEnrollment(track.Id,
                    strand.Id,
                    currentSemester.Value,
                    currentAcademicYear.Value,
                    cancellationToken);

            EnrollStudentViewModel retVal = new()
            {
                StudentId = request.studentId,
                CurrentSemester = currentSemester.Value,
                CurrentAcademicYear = currentAcademicYear.Value,
                TrackAndStrand = $"{track.Name} - {strand.Name}",
                SectionList = new(),
                Schedules = new()
            };

            if(sectionList is not null)
            {
                retVal.SectionList.AddRange(sectionList
                    .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name)));
            }

            if(scheduleList is not null && scheduleList.Count > 0)
            {
                retVal.Schedules.AddRange(scheduleList
                    .Select(x =>
                    {
                        List<string> days = new();
                        foreach (var day in x.Day.Split(','))
                            days.Add(SchoolDays.Get(day).Name);

                        var timeStartStr = x.TimeStart.Hour < 12 ?
                        $"{x.TimeStart.Hour}:{x.TimeStart.Minute} AM" :
                        (x.TimeStart.Hour == 12 ? $"{x.TimeStart.Hour}:{x.TimeStart.Minute} PM" : $"{x.TimeStart.Hour - 12}:{x.TimeStart.Minute} PM");

                        var timeEndStr = x.TimeEnd.Hour < 12 ?
                            $"{x.TimeEnd.Hour}:{x.TimeEnd.Minute} AM" :
                            (x.TimeEnd.Hour == 12 ? $"{x.TimeEnd.Hour}:{x.TimeEnd.Minute} PM" : $"{x.TimeEnd.Hour - 12}:{x.TimeEnd.Minute} PM");

                        var preRequisites = Task.Run(() => _preRequisiteRepository.GetAllBySubjectId(x.Subject.Id, cancellationToken: cancellationToken)).Result;
                        var preRequisiteSubjNames = new List<string>();

                        if(preRequisites is not null && preRequisites.Count > 0)
                        {
                            foreach (var pr in preRequisites!)
                            {
                                var subj = Task.Run(() => _subjectRepository.GetSubjectById(pr.Id), cancellationToken: cancellationToken).Result;

                                preRequisiteSubjNames.Add(subj!.Name);
                            }
                        }

                        return new EnrollStudentScheduleListViewModel()
                        {
                            ScheduleId = x.Id,
                            Subject = $"[{x.Subject.Code}] {x.Subject.Name}",
                            Instructor = x.Instructor.User.MiddleName is null ?
                                $"Prof. {x.Instructor.User.FirstName} {x.Instructor.User.LastName}" :
                                $"Prof. {x.Instructor.User.FirstName} {x.Instructor.User.MiddleName} {x.Instructor.User.LastName}",
                            Days = string.Join(',', days),
                            Time = $"{timeStartStr} - {timeEndStr}",
                            Room = x.RoomNumber,
                            PreReqSubjects = preRequisiteSubjNames,
                            IsSelected = false
                        };
                    }));
            }

            return ResultModel<EnrollStudentViewModel>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Enrollment View");

            error = new(nameof(ex), "Error getting Enrollment View.");

            return ResultModel<EnrollStudentViewModel>.Fail(error);
        }
    }
}
