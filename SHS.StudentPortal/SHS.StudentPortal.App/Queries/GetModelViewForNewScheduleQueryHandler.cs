using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetModelViewForNewScheduleQueryHandler :
    IQueryHandler<GetModelViewForNewScheduleQuery, NewScheduleViewModel>
{
    private readonly ILogger<GetModelViewForNewScheduleQueryHandler> _logger;
    private readonly ISettingRepository _settingRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IInstructorInfoRepository _instructorInfoRepository;

    public GetModelViewForNewScheduleQueryHandler(ILogger<GetModelViewForNewScheduleQueryHandler> logger,
        ISettingRepository settingRepository,
        ISubjectRepository subjectRepository,
        IInstructorInfoRepository instructorInfoRepository)
    {
        _logger = logger;
        _settingRepository = settingRepository;
        _subjectRepository = subjectRepository;
        _instructorInfoRepository = instructorInfoRepository;
    }

    public async Task<ResultModel<NewScheduleViewModel>> Handle(GetModelViewForNewScheduleQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var currentSemesterSetting = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var subjectList = await _subjectRepository.GetAllSubjectBySemester(currentSemesterSetting!.Value, cancellationToken: cancellationToken);
            var instructorList = await _instructorInfoRepository.GetList(true, cancellationToken: cancellationToken);

            NewScheduleViewModel newScheduleViewModel = new()
            {
                SubjectId = string.Empty,
                SubjectList = new List<KeyValuePair<int, string>>(),
                InstructorId = string.Empty,
                InstructorList = new List<KeyValuePair<Guid, string>>(),
                Room = string.Empty,
                Days = string.Empty,
                TimeStartHour = string.Empty,
                TimeStartMinute = string.Empty,
                TimeStartAMPM = "am",
                TimeEndHour = string.Empty,
                TimeEndMinute = string.Empty,
                TimeEndAMPM = "am",
                Remarks = string.Empty
            };

            if(subjectList is not null && subjectList.Count > 0)
                newScheduleViewModel.SubjectList.AddRange(subjectList.Select(x => new KeyValuePair<int, string>(x.Id, x.Name)));

            if (instructorList is not null && instructorList.Count > 0)
                newScheduleViewModel.InstructorList.AddRange(instructorList.Select(x =>
                {
                    return new KeyValuePair<Guid, string>(x.Id, x.User.FirstName == Constants.NotApplicable ? Constants.NotApplicable : $"Prof. {x.User.FirstName} {x.User.LastName}");
                }));

            return ResultModel<NewScheduleViewModel>.Success(newScheduleViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting model view for new schedule.");

            error = new(nameof(Exception), "Error getting view for New Schedule");

            return ResultModel<NewScheduleViewModel>.Fail(error);
        }
    }
}
