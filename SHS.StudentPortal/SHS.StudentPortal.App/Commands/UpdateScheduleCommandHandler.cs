using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdateScheduleCommandHandler
    : ICommandHandler<UpdateScheduleCommand>
{
    private readonly ILogger<UpdateScheduleCommandHandler> _logger;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public UpdateScheduleCommandHandler(ILogger<UpdateScheduleCommandHandler> logger,
        IScheduleRepository scheduleRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var days = new List<string>();
            if (request.view.Day.Contains("M"))
                days.Add(SchoolDays.Monday.Id);

            if (request.view.Day.Contains("TU"))
                days.Add(SchoolDays.Tuesday.Id);

            if (request.view.Day.Contains("W"))
                days.Add(SchoolDays.Wednesday.Id);

            if (request.view.Day.Contains("TH"))
                days.Add(SchoolDays.Thursday.Id);

            if (request.view.Day.Contains("F"))
                days.Add(SchoolDays.Friday.Id);

            if (request.view.Day.Contains("S"))
                days.Add(SchoolDays.Saturday.Id);

            var allSchedulesByDay = await _scheduleRepository.GetListByDay(days, cancellationToken: cancellationToken);

            var timeStartHour = int.Parse(request.view.TimeStartHour) + (request.view.TimeStartAMPM == "pm" ? 12 : 0);
            var newSchedTimeStart = new TimeOnly(timeStartHour, int.Parse(request.view.TimeStartMin));

            var timeEndHour = int.Parse(request.view.TimeEndHour) + ((request.view.TimeEndAMPM == "pm" && int.Parse(request.view.TimeEndHour) < 12) ? 12 : 0);
            var newSchedTimeEnd = new TimeOnly(timeEndHour, int.Parse(request.view.TimeEndMin));

            // Check for conflicts
            if (allSchedulesByDay is not null && allSchedulesByDay.Count > 0)
            {
                if (allSchedulesByDay.Any(x => (x.TimeStart <= newSchedTimeEnd || x.TimeEnd >= newSchedTimeStart) &&
                    x.RoomNumber == request.view.Room &&
                    x.Id != request.view.Id))
                {
                    error = new(nameof(ArgumentException), "Schedule conflicts with existing schedule.");

                    return ResultModel.Fail(error);
                }
            }

            var schedule = await _scheduleRepository.GetScheduleById(request.view.Id, true, cancellationToken);

            if(schedule is null)
            {
                error = new(nameof(KeyNotFoundException), "Original Schedule not found.");

                return ResultModel.Fail(error);
            }

            var updatedSchedule = schedule
                .Update(string.Join(',', days),
                    newSchedTimeStart,
                    newSchedTimeEnd,
                    request.view.Room,
                    request.view.Remarks,
                    Guid.Parse(request.view.InstructorId),
                    int.Parse(request.view.SubjectId),
                    request.updatedById);

            await _scheduleRepository.UpdateSchedule(updatedSchedule, cancellationToken);
            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Schedule");

            error = new(nameof(ex), "Error updating Schedule.");

            return ResultModel.Fail(error);
        }
    }
}
