using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetScheduleByIdQueryHandler
    : IQueryHandler<GetScheduleByIdQuery, ViewScheduleViewModel>
{
    private readonly ILogger<GetScheduleByIdQueryHandler> _logger;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly ISettingRepository _settingRepository;

    public GetScheduleByIdQueryHandler(ILogger<GetScheduleByIdQueryHandler> logger,
        IScheduleRepository scheduleRepository,
        ISubjectRepository subjectRepository,
        IInstructorInfoRepository instructorInfoRepository,
        ISettingRepository settingRepository)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _subjectRepository = subjectRepository;
        _instructorInfoRepository = instructorInfoRepository;
        _settingRepository = settingRepository;
    }

    public async Task<ResultModel<ViewScheduleViewModel>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var schedule = await _scheduleRepository.GetScheduleById(request.id, cancellationToken: cancellationToken);

            if(schedule is null)
            {
                error = new(nameof(KeyNotFoundException), "Schedules not found.");

                return ResultModel<ViewScheduleViewModel>.Fail(error);
            }

            var daysStr = new List<string>();

            var daysArr = schedule.Day.Split(',');

            foreach (var day in daysArr) 
            {
                daysStr.Add(SchoolDays.Get(day).Name);
            }

            var retVal = new ViewScheduleViewModel()
            {
                Id = schedule.Id,
                SubjectId = schedule.SubjectId.ToString(),
                SubjectList = new(),
                InstructorId = schedule.InstructorId.ToString(),
                InstructorList = new(),
                Room = schedule.RoomNumber,
                Days = string.Join(',', daysStr),
                TimeStartHour = (schedule.TimeStart.Hour > 12 ? schedule.TimeStart.Hour - 12 : schedule.TimeStart.Hour).ToString(),
                TimeStartMinute = schedule.TimeStart.Minute < 10 ? $"0{schedule.TimeStart.Minute}" : schedule.TimeStart.Minute.ToString(),
                TimeStartAMPM = schedule.TimeStart.Hour >= 12 ? "PM" : "AM",
                TimeEndHour = (schedule.TimeEnd.Hour > 12 ? schedule.TimeEnd.Hour - 12 : schedule.TimeEnd.Hour).ToString(),
                TimeEndMinute = schedule.TimeEnd.Minute < 10 ? $"0{schedule.TimeEnd.Minute}" : schedule.TimeEnd.Minute.ToString(),
                TimeEndAMPM = schedule.TimeEnd.Hour >= 12 ? "PM" : "AM",
                Remarks = schedule.Remarks ?? string.Empty
            };

            var currentSemesterSetting = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var subjectList = await _subjectRepository.GetAllSubjectBySemester(currentSemesterSetting!.Value, cancellationToken: cancellationToken);
            var instructorList = await _instructorInfoRepository.GetList(true, cancellationToken: cancellationToken);

            if (subjectList is not null && subjectList.Count > 0)
                retVal.SubjectList.AddRange(subjectList.Select(x => new KeyValuePair<int, string>(x.Id, x.Name)));

            if (instructorList is not null && instructorList.Count > 0)
                retVal.InstructorList.AddRange(instructorList.Select(x =>
                {
                    return new KeyValuePair<Guid, string>(x.Id, x.User.FirstName == Constants.NotApplicable ? Constants.NotApplicable : $"Prof. {x.User.FirstName} {x.User.LastName}");
                }));

            return ResultModel<ViewScheduleViewModel>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule details");

            error = new(nameof(ex), "Error getting Schedule details.");

            return ResultModel<ViewScheduleViewModel>.Fail(error);
        }
    }
}
