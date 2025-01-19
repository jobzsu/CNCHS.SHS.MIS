using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;
using System.Text;

namespace SHS.StudentPortal.App.Commands;

internal sealed class CreateScheduleCommandHandler
    : ICommandHandler<CreateScheduleCommand>
{
    private readonly ILogger<CreateScheduleCommandHandler> _logger;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public CreateScheduleCommandHandler(ILogger<CreateScheduleCommandHandler> logger,
        IScheduleRepository scheduleRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
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

            var timeStartHour = request.view.TimeStartAMPM == "am" ?
                int.Parse(request.view.TimeStartHour) : (int.Parse(request.view.TimeStartHour) < 12 ? int.Parse(request.view.TimeStartHour) + 12 : int.Parse(request.view.TimeStartHour));
            var newSchedTimeStart = new TimeOnly(timeStartHour, int.Parse(request.view.TimeStartMin));

            var timeEndHour = request.view.TimeEndAMPM == "am" ?
                int.Parse(request.view.TimeEndHour) : (int.Parse(request.view.TimeEndHour) < 12 ? int.Parse(request.view.TimeEndHour) + 12 : int.Parse(request.view.TimeEndHour));
            var newSchedTimeEnd = new TimeOnly(timeEndHour, int.Parse(request.view.TimeEndMin));

            // Get all schedules with the same days
            var allSchedulesByDay = await _scheduleRepository.GetListByDay(days, cancellationToken: cancellationToken);

            // Check for conflicts
            if (allSchedulesByDay is not null && allSchedulesByDay.Count > 0)
            {
                // Check if any conflicts w/ Room & Time
                var conflictScheds = allSchedulesByDay
                    .Where(s => s.RoomNumber.ToLower() == request.view.Room.ToLower() &&
                              ((s.TimeEnd > newSchedTimeStart) ||
                               (s.TimeEnd > newSchedTimeEnd)) ||
                               (s.TimeStart == newSchedTimeStart))
                    .ToList();

                if(conflictScheds.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    
                    conflictScheds.ForEach(cs =>
                    {
                        sb.AppendLine($"[{cs.Subject.Code}] [{cs.RoomNumber}] [{cs.TimeStart}-{cs.TimeEnd}]");
                    });

                    error = new(nameof(ArgumentException), $"Conflicting Schedules found!\n{sb.ToString()}");

                    return ResultModel.Fail(error);
                }
            }

            var newSchedule = new Schedule()
                .Create(string.Join(',', days),
                    newSchedTimeStart,
                    newSchedTimeEnd,
                    request.view.Room,
                    request.view.Remarks,
                    Guid.Parse(request.view.InstructorId),
                    int.Parse(request.view.SubjectId),
                    request.createdById);

            await _scheduleRepository.CreateSchedule(newSchedule, cancellationToken);
            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule.");

            error = new(nameof(ex), "Error creating schedule.");

            return ResultModel.Fail(error);
        }
    }


}
