using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;
using System.Text;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpsertScheduleCommandHandler
    : ICommandHandler<UpsertScheduleCommand>
{
    private readonly ILogger<UpsertScheduleCommandHandler> _logger;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IBaseUnitOfWork _unitOfWork;

    public UpsertScheduleCommandHandler(ILogger<UpsertScheduleCommandHandler> logger,
        IScheduleRepository scheduleRepository,
        IBaseUnitOfWork unitOfWork)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultModel> Handle(UpsertScheduleCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            // Get all schedules with the same days
            var allSchedulesByDay = await _scheduleRepository.GetListByDay(request.view.Days!, cancellationToken: cancellationToken);

            var newTimeStart = new TimeOnly(request.view.TimeStart!.Value.Hour, request.view.TimeStart!.Value.Minute);
            var newTimeEnd = new TimeOnly(request.view.TimeEnd!.Value.Hour, request.view.TimeEnd!.Value.Minute);

            // Check for conflicts
            if (allSchedulesByDay is not null && allSchedulesByDay.Count > 0)
            {
                // Check if any conflicts w/ Room & Time
                var conflictScheds = allSchedulesByDay
                    .Where(s => s.Id != request.view.Id &&
                                s.RoomNumber.ToLower() == request.view.Room.ToLower() &&
                              ((s.TimeEnd > newTimeStart) ||
                               (s.TimeEnd > newTimeEnd) ||
                               (s.TimeStart == newTimeStart)))
                    .ToList();

                if (conflictScheds.Any())
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

            if (request.view.Id == 0)
            {
                var newSchedule = new Schedule()
                {
                    Day = string.Join(',', request.view.Days!),
                    TimeStart = newTimeStart,
                    TimeEnd = newTimeEnd,
                    RoomNumber = request.view.Room,
                    Remarks = string.IsNullOrWhiteSpace(request.view.Remarks) ? null : request.view.Remarks,
                    InstructorId = request.view.InstructorId!.Value,
                    SubjectId = request.view.SubjectId!.Value,

                    CreatedById = request.actionById,
                    CreatedDate = DateTime.UtcNow
                };

                var txn = _unitOfWork.BeginTransaction();

                try
                {
                    await _scheduleRepository.CreateSchedule(newSchedule, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
            else
            {
                var scheduleToUpdate = await _scheduleRepository.GetScheduleById(request.view.Id, true, cancellationToken);

                if (scheduleToUpdate is null)
                {
                    error = new(nameof(KeyNotFoundException), "Schedule not found.");

                    return ResultModel.Fail(error);
                }

                scheduleToUpdate.SubjectId = request.view.SubjectId!.Value;
                scheduleToUpdate.InstructorId = request.view.InstructorId!.Value;
                scheduleToUpdate.RoomNumber = request.view.Room;
                scheduleToUpdate.Day = string.Join(',', request.view.Days!);
                scheduleToUpdate.TimeStart = newTimeStart;
                scheduleToUpdate.TimeEnd = newTimeEnd;
                scheduleToUpdate.Remarks = string.IsNullOrWhiteSpace(request.view.Remarks) ? null : request.view.Remarks;

                scheduleToUpdate.ModifiedById = request.actionById;
                scheduleToUpdate.ModifiedDate = DateTime.UtcNow;

                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    await _scheduleRepository.UpdateSchedule(scheduleToUpdate, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Saving Schedule");

            error = new(nameof(Exception), "Error Saving Schedule");

            return ResultModel.Fail(error);
        }
    }
}
