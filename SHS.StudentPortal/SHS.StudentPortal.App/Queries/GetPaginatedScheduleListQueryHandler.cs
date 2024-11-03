using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;
using System.Text;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetPaginatedScheduleListQueryHandler
    : IQueryHandler<GetPaginatedScheduleListQuery, SchedulePaginatedList>
{
    private readonly ILogger<GetPaginatedScheduleListQueryHandler> _logger;
    private readonly ISettingRepository _settingRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public GetPaginatedScheduleListQueryHandler(ILogger<GetPaginatedScheduleListQueryHandler> logger,
        ISettingRepository settingRepository,
        IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _settingRepository = settingRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<ResultModel<SchedulePaginatedList>> Handle(GetPaginatedScheduleListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel err;

        try
        {
            var track = Track.GetTrack(request.filter.Track ?? string.Empty).Id;
            var strand = Strand.GetStrand(request.filter.Strand ?? string.Empty).Id;

            var semesterSetting = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var academicYearSetting = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            var daysList = new List<string>();

            if (request.filter.Days.Contains("M"))
                daysList.Add(SchoolDays.Monday.Id);

            if (request.filter.Days.Contains("TU"))
                daysList.Add(SchoolDays.Tuesday.Id);

            if (request.filter.Days.Contains("W"))
                daysList.Add(SchoolDays.Wednesday.Id);

            if (request.filter.Days.Contains("TH"))
                daysList.Add(SchoolDays.Thursday.Id);

            if (request.filter.Days.Contains("F"))
                daysList.Add(SchoolDays.Friday.Id);

            var scheduleList = await _scheduleRepository.GetListViaFilter(request.filter.SearchKeyword,
                daysList,
                track,
                strand,
                semesterSetting!.Value,
                academicYearSetting!.Value,
                cancellationToken);

            if (scheduleList is null || scheduleList.Count() == 0)
            {
                var paginatedList = PaginatedList<ScheduleListViewModel>
                    .Create(new List<ScheduleListViewModel>().AsQueryable(), 1, 10);

                SchedulePaginatedList retVal = new() 
                { 
                    ScheduleList = paginatedList, 
                    CurrentSemester = semesterSetting.Value, 
                    CurrentAcademicYear = academicYearSetting.Value 
                };
                
                return ResultModel<SchedulePaginatedList>.Success(retVal);
            }
            else
            {
                var scheduleListViewModel = new List<ScheduleListViewModel>();

                scheduleListViewModel.AddRange(scheduleList.Select(x => 
                {
                    var trackStrandSplitStr = x.Subject.TrackAndStrand.Split('-');

                    var trackId = trackStrandSplitStr![0];

                    var trackStr = string.Empty;
                    if (trackId == Track.AcademicTrack.Id)
                        trackStr = "Academic";
                    else if (trackId == Track.ArtsAndDesignTrack.Id)
                        trackStr = "Arts & Design";
                    else if (trackId == Track.SportsTrack.Id)
                        trackStr = "Sports";
                    else
                        trackStr = "TVL";

                    var strandStr = Strand.GetStrand(trackStrandSplitStr![1]).IsPlaceholder ?
                        Strand.Placeholder.Name : 
                        Strand.GetStrand(trackStrandSplitStr![1]).Name;

                    var timeStartStr = x.TimeStart.Hour < 12 ? 
                        $"{x.TimeStart.Hour}:{x.TimeStart.Minute} AM" : 
                        (x.TimeStart.Hour == 12 ? $"{x.TimeStart.Hour}:{x.TimeStart.Minute} PM" : $"{x.TimeStart.Hour - 12}:{x.TimeStart.Minute} PM");

                    var timeEndStr = x.TimeEnd.Hour < 12 ?
                        $"{x.TimeEnd.Hour}:{x.TimeEnd.Minute} AM" :
                        (x.TimeEnd.Hour == 12 ? $"{x.TimeEnd.Hour}:{x.TimeEnd.Minute} PM" : $"{x.TimeEnd.Hour - 12}:{x.TimeEnd.Minute} PM");

                    var daysArr = x.Day.Split(',').ToList();
                    var days = new List<string>();

                    foreach (var d in daysArr)
                    {
                        days.Add(SchoolDays.Get(d).Name);
                    }

                    return new ScheduleListViewModel()
                    {
                        Id = x.Id,
                        Days = string.Join(',', days),
                        Subject = x.Subject.Name.ToUpper(),
                        TrackAndStrand = $"{trackStr} - {strandStr}",
                        Instructor = x.Instructor.User.MiddleName == null ? 
                            $"Prof. {x.Instructor.User.FirstName} {x.Instructor.User.LastName}" :
                            $"Prof. {x.Instructor.User.FirstName} {x.Instructor.User.MiddleName} {x.Instructor.User.LastName}",
                        Time = $"{timeStartStr} - {timeEndStr}",
                        RoomNumber = x.RoomNumber.ToUpper()
                    };
                }));

                var paginatedList = PaginatedList<ScheduleListViewModel>
                    .Create(scheduleListViewModel.AsQueryable(), request.filter.PageNumber, 10);

                var schedulePaginatedList = new SchedulePaginatedList
                {
                    CurrentSemester = semesterSetting.Value,
                    CurrentAcademicYear = academicYearSetting.Value,
                    ScheduleList = paginatedList
                };

                return ResultModel<SchedulePaginatedList>.Success(schedulePaginatedList);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated schedule list.");

            err = new(nameof(ex), "Error getting paginated schedule list.");

            return ResultModel<SchedulePaginatedList>.Fail(err);
        }
    }
}
