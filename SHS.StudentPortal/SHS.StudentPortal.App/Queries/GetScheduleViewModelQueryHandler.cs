using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetScheduleViewModelQueryHandler
    : IQueryHandler<GetScheduleViewModelQuery, UpsertScheduleViewModel>
{
    private readonly ILogger<GetScheduleViewModelQueryHandler> _logger;
    private readonly IScheduleRepository _scheduleRepository;

    public GetScheduleViewModelQueryHandler(ILogger<GetScheduleViewModelQueryHandler> logger, 
        IScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<ResultModel<UpsertScheduleViewModel>> Handle(GetScheduleViewModelQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var schedule = await _scheduleRepository.GetScheduleById(request.scheduleId, cancellationToken: cancellationToken);

            if(schedule is null)
            {
                error = new(nameof(KeyNotFoundException), "Schedule not found.");

                return ResultModel<UpsertScheduleViewModel>.Fail(error);
            }

            var retVal = new UpsertScheduleViewModel()
            {
                Id = schedule.Id,
                SubjectId = schedule.SubjectId,
                InstructorId = schedule.InstructorId,
                Room = schedule.RoomNumber,
                Days = schedule.Day.Split(',').ToList(),
                TimeStart = new DateTime(new DateOnly(), schedule.TimeStart),
                TimeEnd = new DateTime(new DateOnly(), schedule.TimeEnd),
                Remarks = schedule.Remarks ?? string.Empty,
            };

            return ResultModel<UpsertScheduleViewModel>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Schedule");

            error = new(nameof(Exception), "Error retrieving Schedule");

            return ResultModel<UpsertScheduleViewModel>.Fail(error);
        }
    }
}
